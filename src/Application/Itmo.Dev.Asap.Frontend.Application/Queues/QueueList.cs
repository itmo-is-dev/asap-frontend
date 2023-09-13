using Itmo.Dev.Asap.Frontend.Application.Abstractions.Queues;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.Queues.Events;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.Queues.Models;
using Itmo.Dev.Asap.Frontend.Application.Events;
using Itmo.Dev.Asap.Frontend.Application.Extensions;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace Itmo.Dev.Asap.Frontend.Application.Queues;

public class QueueList : IQueueList
{
    private readonly IDisposable _disposable;

    public QueueList(Guid subjectCourseId, IEventProvider provider)
    {
        _disposable = Disposable.Empty;

        SubjectCourseId = subjectCourseId;

        Items = provider
            .Observe<QueueListUpdated>()
            .Where(x => x.SubjectCourseId.Equals(SubjectCourseId))
            .ReplayEvent(ref _disposable)
            .Select(x => x.StudentGroupIds);
    }

    public Guid SubjectCourseId { get; }

    public IObservable<IEnumerable<QueueListItem>> Items { get; }

    public void Dispose()
    {
        _disposable.Dispose();
    }
}