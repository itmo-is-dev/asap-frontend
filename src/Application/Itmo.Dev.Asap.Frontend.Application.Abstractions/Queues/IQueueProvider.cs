namespace Itmo.Dev.Asap.Frontend.Application.Abstractions.Queues;

public interface IQueueProvider
{
    ValueTask<IQueue> LoadAsync(Guid subjectCourseId, Guid studentGroupId, CancellationToken cancellationToken);
}