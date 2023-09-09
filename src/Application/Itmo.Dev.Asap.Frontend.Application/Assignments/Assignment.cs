using Itmo.Dev.Asap.Frontend.Application.Abstractions.Assignments;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.Assignments.Events;
using Itmo.Dev.Asap.Frontend.Application.Events;
using Itmo.Dev.Asap.Frontend.Application.Extensions;
using Itmo.Dev.Asap.Frontend.Application.Factories;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace Itmo.Dev.Asap.Frontend.Application.Assignments;

internal class Assignment : IAssignment, ICreatableEntity<Assignment, Guid>
{
    private readonly IDisposable _disposable;

    public Assignment(Guid id, IEventProvider provider)
    {
        Id = id;

        _disposable = Disposable.Empty;

        IObservable<AssignmentUpdated> connection = provider
            .Observe<AssignmentUpdated>()
            .Where(x => x.Id.Equals(Id))
            .ReplayEvent(ref _disposable);

        SubjectCourseId = connection.Select(x => x.SubjectCourseId);
        Name = connection.Select(x => x.Name);
        MinPoints = connection.Select(x => x.MinPoints);
        MaxPoints = connection.Select(x => x.MaxPoints);
    }

    public Guid Id { get; }

    public IObservable<Guid> SubjectCourseId { get; }

    public IObservable<string> Name { get; }

    public IObservable<double> MinPoints { get; }

    public IObservable<double> MaxPoints { get; }

    public static Assignment Create(Guid identifier, IEventProvider provider)
    {
        return new Assignment(identifier, provider);
    }

    public void Dispose()
    {
        _disposable.Dispose();
    }
}