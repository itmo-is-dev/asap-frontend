using Itmo.Dev.Asap.Frontend.Application.Abstractions.Queues;
using Itmo.Dev.Asap.Frontend.Application.Events;
using Itmo.Dev.Asap.Frontend.Application.Factories;

namespace Itmo.Dev.Asap.Frontend.Application.Queues;

public class QueueSubmissionFactory : IQueueSubmissionFactory, IDisposable
{
    private readonly EntityFactory<QueueSubmission, Guid> _factory;

    public QueueSubmissionFactory(IEventProvider provider)
    {
        _factory = new EntityFactory<QueueSubmission, Guid>(provider);
    }

    public IQueueSubmission Create(Guid id)
    {
        return _factory.Create(id);
    }

    public void Dispose()
    {
        _factory.Dispose();
    }
}