namespace Itmo.Dev.Asap.Frontend.Application.Abstractions.Queues;

public interface IQueueService
{
    ValueTask SubscribeToQueueAsync(Guid subjectCourseId, Guid studentGroupId, CancellationToken cancellationToken);

    ValueTask UnsubscribeFromQueueAsync(Guid subjectCourseId, Guid studentGroupId, CancellationToken cancellationToken);

    ValueTask LoadQueueListAsync(Guid subjectCourseId, CancellationToken cancellationToken);
}