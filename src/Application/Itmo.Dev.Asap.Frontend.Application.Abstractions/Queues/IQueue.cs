namespace Itmo.Dev.Asap.Frontend.Application.Abstractions.Queues;

public interface IQueue : IAsyncDisposable
{
    Guid SubjectCourseId { get; }

    Guid StudentGroupId { get; }

    IObservable<IEnumerable<IQueueSubmission>> Submissions { get; }
}