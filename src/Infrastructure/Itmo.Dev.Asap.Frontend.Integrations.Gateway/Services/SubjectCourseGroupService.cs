using Itmo.Dev.Asap.Frontend.Application.Abstractions.Notifications.Events;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.SubjectCourseGroups;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.SubjectCourseGroups.Events;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.SubjectCourseGroups.Models;
using Itmo.Dev.Asap.Frontend.Application.Events;
using Itmo.Dev.Asap.Gateway.Application.Dto.SubjectCourses;
using Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Models;
using Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Models.SubjectCourseGroups;
using Itmo.Dev.Asap.Gateway.Sdk.Clients;
using Itmo.Dev.Asap.Gateway.Sdk.Extensions;
using Refit;

namespace Itmo.Dev.Asap.Frontend.Integrations.Gateway.Services;

internal class SubjectCourseGroupService : ISubjectCourseGroupService
{
    private readonly ISubjectCourseClient _subjectCourseClient;
    private readonly IEventPublisher _publisher;
    private readonly ISubjectCourseGroupFactory _factory;
    private readonly ISubjectCourseGroupClient _subjectCourseGroupClient;

    public SubjectCourseGroupService(
        ISubjectCourseClient subjectCourseClient,
        IEventPublisher publisher,
        ISubjectCourseGroupFactory factory,
        ISubjectCourseGroupClient subjectCourseGroupClient)
    {
        _subjectCourseClient = subjectCourseClient;
        _publisher = publisher;
        _factory = factory;
        _subjectCourseGroupClient = subjectCourseGroupClient;
    }

    public async ValueTask LoadForSubjectCourseAsync(Guid subjectCourseId, CancellationToken cancellationToken)
    {
        IApiResponse<IReadOnlyCollection<SubjectCourseGroupDto>> response = await _subjectCourseClient
            .GetGroupsAsync(subjectCourseId, cancellationToken);

        if (response.IsSuccessStatusCode is false || response.Content is null)
        {
            ErrorDetails? details = await response.TryGetErrorDetailsAsync();

            var errorEvent = new ErrorOccured(details?.Message ?? "Failed to load subject course groups");
            _publisher.Publish(errorEvent);

            return;
        }

        ISubjectCourseGroup[] groups = response.Content
            .Select(x => _factory.Create(x.SubjectCourseId, x.StudentGroupId))
            .ToArray();

        IEnumerable<SubjectCourseGroupUpdated> events = response.Content.Select(ToEvent);
        _publisher.Publish(events);

        var evt = new SubjectCourseGroupsLoaded(subjectCourseId, groups);
        _publisher.Publish(evt);
    }

    public async ValueTask<CreateSubjectCourseGroupResult> CreateAsync(
        Guid subjectCourseId,
        IEnumerable<Guid> studentGroupIds,
        CancellationToken cancellationToken)
    {
        var request = new BulkCreateSubjectCourseGroupsRequest(subjectCourseId, studentGroupIds);

        IApiResponse<IReadOnlyCollection<SubjectCourseGroupDto>> response = await _subjectCourseGroupClient
            .CreateAsync(request, cancellationToken);

        if (response.IsSuccessStatusCode is false || response.Content is null)
        {
            ErrorDetails? details = await response.TryGetErrorDetailsAsync();

            var errorEvent = new ErrorOccured(details?.Message ?? "Failed to add groups to subject course");
            _publisher.Publish(errorEvent);

            return new CreateSubjectCourseGroupResult.Failure();
        }

        ISubjectCourseGroup[] subjectCourseGroups = response.Content
            .Select(g => _factory.Create(g.SubjectCourseId, g.StudentGroupId))
            .ToArray();

        IEnumerable<SubjectCourseGroupUpdated> updatedEvents = response.Content.Select(ToEvent);
        _publisher.Publish(updatedEvents);

        var createdEvent = new SubjectCourseGroupsCreated(subjectCourseId, subjectCourseGroups);
        _publisher.Publish(createdEvent);

        var successEvent = new SuccessfulOperationOccured("Added groups to subject course");
        _publisher.Publish(successEvent);

        return new CreateSubjectCourseGroupResult.Success();
    }

    public async ValueTask<IEnumerable<SubjectCourseGroupInfo>> QueryAsync(
        Guid subjectCourseId,
        IEnumerable<string> names,
        CancellationToken cancellationToken)
    {
        var request = new QuerySubjectCourseGroupsRequest(
            subjectCourseId,
            Array.Empty<Guid>(),
            names);

        IApiResponse<IEnumerable<SubjectCourseGroupDto>> response = await _subjectCourseGroupClient
            .QueryAsync(request, cancellationToken);

        if (response.IsSuccessStatusCode is false || response.Content is null)
        {
            ErrorDetails? details = await response.TryGetErrorDetailsAsync();

            var errorEvent = new ErrorOccured(details?.Message ?? "Failed to query groups");
            _publisher.Publish(errorEvent);

            return Enumerable.Empty<SubjectCourseGroupInfo>();
        }

        return response.Content
            .Select(group => new SubjectCourseGroupInfo(group.StudentGroupId, group.StudentGroupName));
    }

    private static SubjectCourseGroupUpdated ToEvent(SubjectCourseGroupDto dto)
    {
        return new SubjectCourseGroupUpdated(dto.SubjectCourseId, dto.StudentGroupId, dto.StudentGroupName);
    }
}