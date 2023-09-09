using Itmo.Dev.Asap.Frontend.Application.Abstractions.Errors.Events;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.SubjectCourses;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.SubjectCourses.Associations;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.SubjectCourses.Events;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.SubjectCourses.Models;
using Itmo.Dev.Asap.Frontend.Application.Events;
using Itmo.Dev.Asap.Frontend.Integrations.Gateway.Mapping;
using Itmo.Dev.Asap.Gateway.Application.Dto.SubjectCourses;
using Itmo.Dev.Asap.Gateway.Sdk.Clients;
using Refit;

namespace Itmo.Dev.Asap.Frontend.Integrations.Gateway.Services;

internal class SubjectCourseService : ISubjectCourseService
{
    private readonly ISubjectClient _subjectClient;
    private readonly ISubjectCourseClient _subjectCourseClient;
    private readonly IEventPublisher _publisher;
    private readonly ISubjectCourseFactory _subjectCourseFactory;

    public SubjectCourseService(
        ISubjectClient subjectClient,
        ISubjectCourseClient subjectCourseClient,
        IEventPublisher publisher,
        ISubjectCourseFactory subjectCourseFactory)
    {
        _subjectCourseClient = subjectCourseClient;
        _publisher = publisher;
        _subjectCourseFactory = subjectCourseFactory;
        _subjectClient = subjectClient;
    }

    public async ValueTask LoadForSubjectAsync(Guid subjectId, CancellationToken cancellationToken)
    {
        IApiResponse<IReadOnlyCollection<SubjectCourseDto>> response = await _subjectClient
            .GetCoursesAsync(subjectId, cancellationToken);

        if (response.IsSuccessStatusCode is false || response.Content is null)
            return;

        ISubjectCourse[] courses = response.Content
            .Select(x => _subjectCourseFactory.Create(x.Id))
            .ToArray();

        IEnumerable<SubjectCourseUpdated> events = response.Content
            .Select(x =>
            {
                SubjectCourseAssociation[] associations = x.Associations.Select(DtoMapper.MapToAssociation).ToArray();
                return new SubjectCourseUpdated(x.Id, x.SubjectId, x.Title, associations);
            });

        _publisher.Publish(events);

        var evt = new SubjectCoursesLoaded(subjectId, courses);
        _publisher.Publish(evt);
    }

    public async ValueTask LoadAsync(Guid subjectCourseId, CancellationToken cancellationToken)
    {
        IApiResponse<SubjectCourseDto> response = await _subjectCourseClient
            .GetByIdAsync(subjectCourseId, cancellationToken);

        if (response.IsSuccessStatusCode is false || response.Content is null)
            return;

        SubjectCourseDto dto = response.Content;

        _subjectCourseFactory.Create(dto.Id);

        SubjectCourseAssociation[] associations = dto.Associations.Select(DtoMapper.MapToAssociation).ToArray();

        var evt = new SubjectCourseUpdated(dto.Id, dto.SubjectId, dto.Title, associations);
        _publisher.Publish(evt);
    }

    public async ValueTask SyncPointsAsync(Guid subjectCourseId, CancellationToken cancellationToken)
    {
        await _subjectCourseClient.ForceSyncPointsAsync(subjectCourseId, cancellationToken);
    }

    public async ValueTask<CreateSubjectCourseResult> CreateAsync(
        CreateSubjectCourseRequest request,
        CancellationToken cancellationToken)
    {
        Asap.Gateway.Presentation.Abstractions.Models.SubjectCourses.CreateSubjectCourseRequest req = request
            .MapToRequest();

        IApiResponse<SubjectCourseDto> response = await _subjectCourseClient.CreateAsync(req, cancellationToken);

        if (response.IsSuccessStatusCode is false || response.Content is null)
        {
            var errorEvent = new ErrorOccured("Failed to create subject course");
            _publisher.Publish(errorEvent);

            return new CreateSubjectCourseResult.Failure();
        }

        ISubjectCourse subjectCourse = _subjectCourseFactory.Create(response.Content.Id);

        var updatedEvent = new SubjectCourseUpdated(
            response.Content.Id,
            response.Content.SubjectId,
            response.Content.Title,
            response.Content.Associations.Select(DtoMapper.MapToAssociation).ToArray());

        _publisher.Publish(updatedEvent);

        var createdEvent = new SubjectCourseCreated(subjectCourse);
        _publisher.Publish(createdEvent);

        return new CreateSubjectCourseResult.Success();
    }
}