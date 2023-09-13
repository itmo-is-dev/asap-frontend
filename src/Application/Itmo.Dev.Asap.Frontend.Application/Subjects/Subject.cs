using Itmo.Dev.Asap.Frontend.Application.Abstractions.Subjects;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.Subjects.Events;
using Itmo.Dev.Asap.Frontend.Application.Events;
using Itmo.Dev.Asap.Frontend.Application.Extensions;
using Itmo.Dev.Asap.Frontend.Application.Factories;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace Itmo.Dev.Asap.Frontend.Application.Subjects;

internal class Subject : ISubject, ICreatableEntity<Subject, Guid>
{
    private readonly IDisposable _disposable;

    public Subject(Guid id, IEventProvider provider)
    {
        _disposable = Disposable.Empty;

        Id = id;

        Name = provider
            .Observe<SubjectUpdated>()
            .Where(x => x.Id.Equals(Id))
            .ReplayEvent(ref _disposable)
            .Select(x => x.Name);
    }

    public Guid Id { get; }

    public IObservable<string> Name { get; }

    public static Subject Create(Guid identifier, IEventProvider provider)
    {
        return new Subject(identifier, provider);
    }

    public void Dispose()
    {
        _disposable.Dispose();
    }
}