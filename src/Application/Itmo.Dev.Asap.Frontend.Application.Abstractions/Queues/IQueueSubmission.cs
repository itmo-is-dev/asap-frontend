using Itmo.Dev.Asap.Frontend.Application.Abstractions.Submissions;

namespace Itmo.Dev.Asap.Frontend.Application.Abstractions.Queues;

public interface IQueueSubmission : IDisposable
{
    Guid Id { get; }

    IObservable<Guid> StudentId { get; }

    IObservable<DateTime?> SubmissionDate { get; }

    IObservable<string> Payload { get; }

    IObservable<string> AssignmentShortName { get; }

    IObservable<SubmissionState> State { get; }
}