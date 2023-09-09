using Itmo.Dev.Asap.Frontend.Application.Abstractions.Assignments;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.Assignments.Events;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.Assignments.Models;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.Errors.Events;
using Itmo.Dev.Asap.Frontend.Application.Events;
using Itmo.Dev.Asap.Gateway.Application.Dto.Study;
using Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Models;
using Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Models.Assignments;
using Itmo.Dev.Asap.Gateway.Sdk.Clients;
using Itmo.Dev.Asap.Gateway.Sdk.Extensions;
using Refit;

namespace Itmo.Dev.Asap.Frontend.Integrations.Gateway.Services;

internal class AssignmentService : IAssignmentService
{
    private readonly IAssignmentClient _assignmentClient;
    private readonly ISubjectCourseClient _subjectCourseClient;
    private readonly IEventPublisher _publisher;
    private readonly IAssignmentFactory _factory;

    public AssignmentService(
        IAssignmentClient assignmentClient,
        IEventPublisher publisher,
        IAssignmentFactory factory,
        ISubjectCourseClient subjectCourseClient)
    {
        _assignmentClient = assignmentClient;
        _publisher = publisher;
        _factory = factory;
        _subjectCourseClient = subjectCourseClient;
    }

    public async Task LoadAsync(Guid assignmentId, CancellationToken cancellationToken)
    {
        IApiResponse<AssignmentDto> response = await _assignmentClient.GetByIdAsync(assignmentId, cancellationToken);

        if (response.IsSuccessStatusCode is false || response.Content is null)
            return;

        AssignmentDto dto = response.Content;
        _factory.Create(dto.Id);

        AssignmentUpdated evt = ToEvent(dto);
        _publisher.Publish(evt);
    }

    public async Task LoadForSubjectCourseId(Guid subjectCourseId, CancellationToken cancellationToken)
    {
        IApiResponse<IReadOnlyCollection<AssignmentDto>> response = await _subjectCourseClient
            .GetAssignmentsAsync(subjectCourseId, cancellationToken);

        if (response.IsSuccessStatusCode is false || response.Content is null)
            return;

        IAssignment[] assignments = response.Content
            .Select(x => _factory.Create(x.Id))
            .ToArray();

        IEnumerable<AssignmentUpdated> events = response.Content.Select(ToEvent);
        _publisher.Publish(events);

        var evt = new AssignmentsLoaded(subjectCourseId, assignments);
        _publisher.Publish(evt);
    }

    public async Task<CreateAssignmentResult> CreateAsync(
        Guid subjectCourseId,
        string name,
        string shortName,
        int order,
        double minPoints,
        double maxPoints,
        CancellationToken cancellationToken)
    {
        var request = new CreateAssignmentRequest(subjectCourseId, name, shortName, order, minPoints, maxPoints);
        IApiResponse<AssignmentDto> response = await _assignmentClient.CreateAsync(request, cancellationToken);

        if (response.IsSuccessStatusCode is false || response.Content is null)
        {
            var errorEvent = new ErrorOccured("Failed to create assignment");
            _publisher.Publish(errorEvent);

            return new CreateAssignmentResult.Failure();
        }

        IAssignment assignment = _factory.Create(response.Content.Id);

        AssignmentUpdated updatedEvent = ToEvent(response.Content);
        _publisher.Publish(updatedEvent);

        var createdEvent = new AssignmentCreated(assignment);
        _publisher.Publish(createdEvent);

        return new CreateAssignmentResult.Success();
    }

    public async ValueTask<UpdateMinPointsResult> UpdateMinPointsAsync(
        Guid assignmentId,
        double points,
        CancellationToken cancellationToken)
    {
        IApiResponse<AssignmentDto> response = await _assignmentClient.UpdatePointsAsync(
            assignmentId,
            minPoints: points,
            maxPoints: null,
            cancellationToken);

        if (response.IsSuccessStatusCode is false || response.Content is null)
        {
            ErrorDetails? details = await response.TryGetErrorDetailsAsync();

            var errorEvent = new ErrorOccured(details?.Message ?? "Failed to update min points");
            _publisher.Publish(errorEvent);

            return new UpdateMinPointsResult.Failure();
        }

        AssignmentUpdated updatedEvent = ToEvent(response.Content);
        _publisher.Publish(updatedEvent);

        return new UpdateMinPointsResult.Success();
    }

    public async ValueTask<UpdateMaxPointsResult> UpdateMaxPointsAsync(
        Guid assignmentId,
        double points,
        CancellationToken cancellationToken)
    {
        IApiResponse<AssignmentDto> response = await _assignmentClient.UpdatePointsAsync(
            assignmentId,
            minPoints: null,
            maxPoints: points,
            cancellationToken);

        if (response.IsSuccessStatusCode is false || response.Content is null)
        {
            ErrorDetails? details = await response.TryGetErrorDetailsAsync();

            var errorEvent = new ErrorOccured(details?.Message ?? "Failed to update max points");
            _publisher.Publish(errorEvent);

            return new UpdateMaxPointsResult.Failure();
        }

        AssignmentUpdated updatedEvent = ToEvent(response.Content);
        _publisher.Publish(updatedEvent);

        return new UpdateMaxPointsResult.Success();
    }

    private static AssignmentUpdated ToEvent(AssignmentDto dto)
    {
        return new AssignmentUpdated(dto.Id, dto.SubjectCourseId, dto.Title, dto.MinPoints, dto.MaxPoints);
    }
}