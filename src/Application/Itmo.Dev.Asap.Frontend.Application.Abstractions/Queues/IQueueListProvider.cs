namespace Itmo.Dev.Asap.Frontend.Application.Abstractions.Queues;

public interface IQueueListProvider
{
    ValueTask<IQueueList> LoadAsync(Guid subjectCourseId, CancellationToken cancellationToken);
}