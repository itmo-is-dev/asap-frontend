using Itmo.Dev.Asap.Frontend.Application.Abstractions.Queues;
using Itmo.Dev.Asap.Frontend.Application.Events;

namespace Itmo.Dev.Asap.Frontend.Application.Queues;

public class QueueProvider : IQueueProvider
{
    private readonly IQueueService _service;
    private readonly IEventProvider _provider;

    public QueueProvider(IQueueService service, IEventProvider provider)
    {
        _service = service;
        _provider = provider;
    }

    public async ValueTask<IQueue> LoadAsync(Guid subjectCourseId, Guid studentGroupId, CancellationToken cancellationToken)
    {
        var queue = new Queue(subjectCourseId, studentGroupId, _service, _provider);
        await _service.SubscribeToQueueAsync(subjectCourseId, studentGroupId, cancellationToken);

        return queue;
    }
}