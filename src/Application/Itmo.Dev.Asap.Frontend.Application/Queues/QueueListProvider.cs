using Itmo.Dev.Asap.Frontend.Application.Abstractions.Queues;
using Itmo.Dev.Asap.Frontend.Application.Events;

namespace Itmo.Dev.Asap.Frontend.Application.Queues;

public class QueueListProvider : IQueueListProvider
{
    private readonly IQueueService _service;
    private readonly IEventProvider _provider;

    public QueueListProvider(IQueueService service, IEventProvider provider)
    {
        _service = service;
        _provider = provider;
    }

    public async ValueTask<IQueueList> LoadAsync(Guid subjectCourseId, CancellationToken cancellationToken)
    {
        var list = new QueueList(subjectCourseId, _provider);
        await _service.LoadQueueListAsync(subjectCourseId, cancellationToken);

        return list;
    }
}