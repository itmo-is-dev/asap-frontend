namespace Itmo.Dev.Asap.Frontend.Application.Abstractions.Queues;

public interface IQueueSubmissionFactory
{
    IQueueSubmission Create(Guid id);
}