using Itmo.Dev.Asap.Frontend.Application.Abstractions.Queues;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.Queues.Events;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.Submissions;
using Itmo.Dev.Asap.Frontend.Application.Events;
using Itmo.Dev.Asap.Frontend.Application.Extensions;
using Itmo.Dev.Asap.Frontend.Application.Factories;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace Itmo.Dev.Asap.Frontend.Application.Queues;

public class QueueSubmission : IQueueSubmission, ICreatableEntity<QueueSubmission, Guid>
{
    private readonly IDisposable _disposable;

    public QueueSubmission(Guid id, IEventProvider provider)
    {
        Id = id;

        _disposable = Disposable.Empty;

        IObservable<QueueSubmissionUpdated> connection = provider
            .Observe<QueueSubmissionUpdated>()
            .Where(x => x.Id.Equals(Id))
            .ReplayEvent(ref _disposable);

        StudentId = connection.Select(x => x.StudentId);
        SubmissionDate = connection.Select(x => x.SubmissionDate);
        Payload = connection.Select(x => x.Payload);
        AssignmentShortName = connection.Select(x => x.AssignmentShortName);
        State = connection.Select(x => x.State);
    }

    public Guid Id { get; }

    public IObservable<Guid> StudentId { get; }

    public IObservable<DateTime?> SubmissionDate { get; }

    public IObservable<string> Payload { get; }

    public IObservable<string> AssignmentShortName { get; }

    public IObservable<SubmissionState> State { get; }

    public static QueueSubmission Create(Guid identifier, IEventProvider provider)
    {
        return new QueueSubmission(identifier, provider);
    }

    public void Dispose()
    {
        _disposable.Dispose();
    }
}