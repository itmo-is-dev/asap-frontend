using Itmo.Dev.Asap.Frontend.Application.Abstractions.Queues;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.Queues.Events;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.Queues.Models;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.StudentGroups;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.StudentGroups.Events;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.Students;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.Students.Events;
using Itmo.Dev.Asap.Frontend.Application.Events;
using Itmo.Dev.Asap.Frontend.Integrations.Gateway.Mapping;
using Itmo.Dev.Asap.Gateway.Application.Dto.SubjectCourses;
using Itmo.Dev.Asap.Gateway.Application.Dto.Tables;
using Itmo.Dev.Asap.Gateway.Application.Dto.Users;
using Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Models.Queue;
using Itmo.Dev.Asap.Gateway.Sdk.Clients;
using Refit;

namespace Itmo.Dev.Asap.Frontend.Integrations.Gateway.Services;

public class QueueService : IQueueService, IDisposable
{
    private readonly IQueueClient _queueClient;
    private readonly ISubjectCourseClient _subjectCourseClient;
    private readonly IEventPublisher _publisher;
    private readonly IQueueSubmissionFactory _submissionFactory;
    private readonly IStudentFactory _studentFactory;
    private readonly IStudentGroupFactory _studentGroupFactory;

    private readonly IDisposable _disposable;

    public QueueService(
        IQueueClient queueClient,
        IEventPublisher publisher,
        IQueueSubmissionFactory submissionFactory,
        ISubjectCourseClient subjectCourseClient,
        IStudentFactory studentFactory,
        IStudentGroupFactory studentGroupFactory)
    {
        _queueClient = queueClient;
        _publisher = publisher;
        _submissionFactory = submissionFactory;
        _subjectCourseClient = subjectCourseClient;
        _studentFactory = studentFactory;
        _studentGroupFactory = studentGroupFactory;

        _disposable = queueClient.QueueUpdates.Subscribe(OnQueueUpdated);
    }

    public async ValueTask SubscribeToQueueAsync(
        Guid subjectCourseId,
        Guid studentGroupId,
        CancellationToken cancellationToken)
    {
        await LoadQueueAsync(subjectCourseId, studentGroupId, cancellationToken);
        await _queueClient.SubscribeToQueueUpdates(subjectCourseId, studentGroupId, cancellationToken);
    }

    public async ValueTask UnsubscribeFromQueueAsync(
        Guid subjectCourseId,
        Guid studentGroupId,
        CancellationToken cancellationToken)
    {
        await _queueClient.UnsubscribeFromQueueUpdates(subjectCourseId, studentGroupId, cancellationToken);
    }

    public async ValueTask LoadQueueListAsync(Guid subjectCourseId, CancellationToken cancellationToken)
    {
        IApiResponse<IReadOnlyCollection<SubjectCourseGroupDto>> response = await _subjectCourseClient
            .GetGroupsAsync(subjectCourseId, cancellationToken);

        if (response.IsSuccessStatusCode is false || response.Content is null)
            return;

        QueueListItem[] items = response.Content
            .Select(x => new QueueListItem(x.StudentGroupId, x.StudentGroupName))
            .ToArray();

        var evt = new QueueListUpdated(subjectCourseId, items);
        _publisher.Publish(evt);
    }

    public void Dispose()
    {
        _disposable.Dispose();
    }

    private async Task LoadQueueAsync(Guid subjectCourseId, Guid studentGroupId, CancellationToken cancellationToken)
    {
        IApiResponse<SubmissionsQueueDto> response = await _subjectCourseClient
            .GetStudentGroupQueueAsync(subjectCourseId, studentGroupId, cancellationToken);

        if (response.IsSuccessStatusCode is false)
            return;

        IQueueSubmission[] submissions = response.Content.Submissions
            .Select(x => _submissionFactory.Create(x.Id))
            .ToArray();

        foreach (StudentDto student in response.Content.Students)
        {
            _studentFactory.Create(student.User.Id);
        }

        IEnumerable<QueueSubmissionUpdated> submissionEvents = response.Content.Submissions
            .Select(x => x.MapToEvent());

        IEnumerable<StudentUpdated> studentEvents = response.Content.Students
            .Select(x => x.MapToUpdatedEvent(_studentGroupFactory));

        IEnumerable<StudentGroupUpdated> groupEvents = response.Content.Students
            .Where(x => x.GroupId is not null)
            .DistinctBy(x => x.GroupId)
            .Select(x => new StudentGroupUpdated(x.GroupId!.Value, x.GroupName));

        _publisher.Publish(submissionEvents);
        _publisher.Publish(studentEvents);
        _publisher.Publish(groupEvents);

        var queueEvent = new QueueUpdated(subjectCourseId, studentGroupId, submissions);
        _publisher.Publish(queueEvent);
    }

    private void OnQueueUpdated(QueueUpdatedMessage message)
    {
        IQueueSubmission[] submissions = message.SubmissionQueue.Submissions
            .Select(x => _submissionFactory.Create(x.Id))
            .ToArray();

        foreach (KeyValuePair<Guid, StudentMessage> student in message.SubmissionQueue.Students)
        {
            _studentFactory.Create(student.Value.User.Id);
        }

        IEnumerable<QueueSubmissionUpdated> submissionEvents = message.SubmissionQueue.Submissions
            .Select(x => x.MapToEvent());

        IEnumerable<StudentUpdated> studentEvents = message.SubmissionQueue.Students.Values
            .Select(x => x.MapToEvent(_studentGroupFactory));

        IEnumerable<StudentGroupUpdated> groupEvents = message.SubmissionQueue.Students.Values
            .Where(x => x.GroupId is not null)
            .DistinctBy(x => x.GroupId)
            .Select(x => new StudentGroupUpdated(x.GroupId!.Value, x.GroupName));

        _publisher.Publish(submissionEvents);
        _publisher.Publish(studentEvents);
        _publisher.Publish(groupEvents);

        var queueEvent = new QueueUpdated(message.SubjectCourseId, message.StudentGroupId, submissions);
        _publisher.Publish(queueEvent);
    }
}