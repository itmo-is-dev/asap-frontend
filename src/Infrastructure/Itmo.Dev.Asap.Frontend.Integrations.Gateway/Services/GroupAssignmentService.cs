using Itmo.Dev.Asap.Frontend.Application.Abstractions.GroupAssignments;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.GroupAssignments.Events;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.GroupAssignments.Models;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.Notifications.Events;
using Itmo.Dev.Asap.Frontend.Application.Events;
using Itmo.Dev.Asap.Gateway.Application.Dto.Study;
using Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Models;
using Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Models.GroupAssignments;
using Itmo.Dev.Asap.Gateway.Sdk.Clients;
using Itmo.Dev.Asap.Gateway.Sdk.Extensions;
using Refit;

namespace Itmo.Dev.Asap.Frontend.Integrations.Gateway.Services;

internal class GroupAssignmentService : IGroupAssignmentService
{
    private readonly IAssignmentClient _assignmentClient;
    private readonly IGroupAssignmentFactory _factory;
    private readonly IEventPublisher _publisher;

    public GroupAssignmentService(
        IAssignmentClient assignmentClient,
        IGroupAssignmentFactory factory,
        IEventPublisher publisher)
    {
        _assignmentClient = assignmentClient;
        _factory = factory;
        _publisher = publisher;
    }

    public async ValueTask LoadForAssignmentAsync(Guid assignmentId, CancellationToken cancellationToken)
    {
        IApiResponse<IReadOnlyCollection<GroupAssignmentDto>> response = await _assignmentClient
            .GetGroupAssignmentsAsync(assignmentId, cancellationToken);

        if (response.IsSuccessStatusCode is false || response.Content is null)
            return;

        IGroupAssignment[] groupAssignments = response.Content
            .Select(x => _factory.Create(x.AssignmentId, x.GroupId))
            .ToArray();

        IEnumerable<GroupAssignmentUpdated> events = response.Content.Select(ToEvent);
        _publisher.Publish(events);

        var evt = new GroupAssignmentsLoaded(assignmentId, groupAssignments);
        _publisher.Publish(evt);
    }

    public async ValueTask<UpdateDeadlineResult> UpdateDeadlineAsync(
        Guid assignmentId,
        Guid studentGroupId,
        DateTime deadline,
        CancellationToken cancellationToken)
    {
        var request = new UpdateGroupAssignmentRequest(deadline);

        IApiResponse<GroupAssignmentDto> response = await _assignmentClient.UpdateGroupAssignmentAsync(
            assignmentId,
            studentGroupId,
            request,
            cancellationToken);

        if (response.IsSuccessStatusCode is false || response.Content is null)
        {
            ErrorDetails? details = await response.TryGetErrorDetailsAsync();

            var errorEvent = new ErrorOccured(details?.Message ?? "Failed to update deadline");
            _publisher.Publish(errorEvent);

            return new UpdateDeadlineResult.Failure();
        }

        var updatedEvent = new GroupAssignmentUpdated(
            response.Content.AssignmentId,
            response.Content.GroupId,
            response.Content.GroupName,
            response.Content.Deadline);

        _publisher.Publish(updatedEvent);

        return new UpdateDeadlineResult.Success();
    }

    public async ValueTask<UpdateDeadlineResult> UpdateDeadlinesAsync(
        Guid assignmentId,
        DateTime deadline,
        IEnumerable<Guid> studentGroupIds,
        CancellationToken cancellationToken)
    {
        var request = new UpdateGroupAssignmentDeadlinesRequest(deadline, studentGroupIds);

        IApiResponse<IReadOnlyCollection<GroupAssignmentDto>> response = await _assignmentClient
            .UpdateGroupAssignmentDeadlinesAsync(assignmentId, request, cancellationToken);

        if (response.IsSuccessStatusCode is false || response.Content is null)
        {
            ErrorDetails? details = await response.TryGetErrorDetailsAsync();

            var errorEvent = new ErrorOccured(details?.Message ?? "Failed to update deadline");
            _publisher.Publish(errorEvent);

            return new UpdateDeadlineResult.Failure();
        }

        IEnumerable<GroupAssignmentUpdated> updatedEvents = response.Content
            .Select(x => new GroupAssignmentUpdated(x.AssignmentId, x.GroupId, x.GroupName, x.Deadline));

        _publisher.Publish(updatedEvents);

        return new UpdateDeadlineResult.Success();
    }

    private static GroupAssignmentUpdated ToEvent(GroupAssignmentDto dto)
    {
        return new GroupAssignmentUpdated(dto.AssignmentId, dto.GroupId, dto.GroupName, dto.Deadline);
    }
}