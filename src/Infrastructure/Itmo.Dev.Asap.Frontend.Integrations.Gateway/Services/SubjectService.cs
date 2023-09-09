using Itmo.Dev.Asap.Frontend.Application.Abstractions.Errors.Events;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.Subjects;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.Subjects.Events;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.Subjects.Models;
using Itmo.Dev.Asap.Frontend.Application.Events;
using Itmo.Dev.Asap.Gateway.Application.Dto.Study;
using Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Models.Subjects;
using Itmo.Dev.Asap.Gateway.Sdk.Clients;
using Refit;

namespace Itmo.Dev.Asap.Frontend.Integrations.Gateway.Services;

internal class SubjectService : ISubjectService
{
    private readonly ISubjectClient _client;
    private readonly IEventPublisher _publisher;
    private readonly ISubjectFactory _subjectFactory;

    public SubjectService(ISubjectClient client, IEventPublisher publisher, ISubjectFactory subjectFactory)
    {
        _client = client;
        _publisher = publisher;
        _subjectFactory = subjectFactory;
    }

    public async Task LoadAsync(CancellationToken cancellationToken)
    {
        IApiResponse<IReadOnlyCollection<SubjectDto>> response = await _client.GetAllAsync(cancellationToken);

        if (response.IsSuccessStatusCode is false || response.Content is null)
            return;

        ISubject[] subjects = response.Content
            .Select(x => _subjectFactory.Create(x.Id))
            .ToArray();

        IEnumerable<SubjectUpdated> events = response.Content
            .Select(x => new SubjectUpdated(x.Id, x.Name));

        _publisher.Publish(events);

        var evt = new SubjectsLoaded(subjects);
        _publisher.Publish(evt);
    }

    public async Task LoadAsync(Guid id, CancellationToken cancellationToken)
    {
        IApiResponse<SubjectDto> response = await _client.GetByIdAsync(id, cancellationToken);

        if (response.IsSuccessStatusCode is false)
            return;

        SubjectDto? dto = response.Content;

        if (dto is null)
            return;

        _subjectFactory.Create(dto.Id);

        var evt = new SubjectUpdated(dto.Id, dto.Name);
        _publisher.Publish(evt);
    }

    public async Task<CreateSubjectResult> CreateAsync(string name, CancellationToken cancellationToken)
    {
        var request = new CreateSubjectRequest(name);
        IApiResponse<SubjectDto> response = await _client.CreateAsync(request, cancellationToken);

        if (response.IsSuccessStatusCode is false || response.Content is null)
        {
            var errorEvent = new ErrorOccured("Failed to create subject");
            _publisher.Publish(errorEvent);

            return new CreateSubjectResult.Failure();
        }

        ISubject subject = _subjectFactory.Create(response.Content.Id);

        var updatedEvent = new SubjectUpdated(
            response.Content.Id,
            response.Content.Name);

        _publisher.Publish(updatedEvent);

        var createdEvent = new SubjectCreated(subject);
        _publisher.Publish(createdEvent);

        return new CreateSubjectResult.Success();
    }
}