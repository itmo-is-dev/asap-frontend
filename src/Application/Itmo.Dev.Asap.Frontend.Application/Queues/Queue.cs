using Itmo.Dev.Asap.Frontend.Application.Abstractions.Queues;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.Queues.Events;
using Itmo.Dev.Asap.Frontend.Application.Events;
using Itmo.Dev.Asap.Frontend.Application.Extensions;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace Itmo.Dev.Asap.Frontend.Application.Queues;

public class Queue : IQueue
{
    private readonly IQueueService _service;
    private readonly IDisposable _disposable;

    public Queue(Guid subjectCourseId, Guid studentGroupId, IQueueService service, IEventProvider provider)
    {
        _service = service;

        _disposable = Disposable.Empty;

        SubjectCourseId = subjectCourseId;
        StudentGroupId = studentGroupId;

        Submissions = provider
            .Observe<QueueUpdated>()
            .Where(x => x.SubjectCourseId.Equals(SubjectCourseId) && x.StudentGroupId.Equals(StudentGroupId))
            .ReplayEvent(ref _disposable)
            .Select(x => x.Submissions);
    }

    public Guid SubjectCourseId { get; }

    public Guid StudentGroupId { get; }

    public IObservable<IEnumerable<IQueueSubmission>> Submissions { get; }

    public async ValueTask DisposeAsync()
    {
        _disposable.Dispose();
        await _service.UnsubscribeFromQueueAsync(SubjectCourseId, StudentGroupId, default);
    }
}