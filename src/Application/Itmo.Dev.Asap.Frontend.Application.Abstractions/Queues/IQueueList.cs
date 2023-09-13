using Itmo.Dev.Asap.Frontend.Application.Abstractions.Queues.Models;

namespace Itmo.Dev.Asap.Frontend.Application.Abstractions.Queues;

public interface IQueueList : IDisposable
{
    Guid SubjectCourseId { get; }

    IObservable<IEnumerable<QueueListItem>> Items { get; }
}