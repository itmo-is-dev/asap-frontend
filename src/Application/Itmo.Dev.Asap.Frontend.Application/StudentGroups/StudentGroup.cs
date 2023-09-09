using Itmo.Dev.Asap.Frontend.Application.Abstractions.StudentGroups;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.StudentGroups.Events;
using Itmo.Dev.Asap.Frontend.Application.Events;
using Itmo.Dev.Asap.Frontend.Application.Extensions;
using Itmo.Dev.Asap.Frontend.Application.Factories;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace Itmo.Dev.Asap.Frontend.Application.StudentGroups;

internal class StudentGroup : IStudentGroup, ICreatableEntity<StudentGroup, Guid>
{
    private readonly IDisposable _disposable;

    public StudentGroup(Guid id, IEventProvider provider)
    {
        Id = id;

        _disposable = Disposable.Empty;

        Name = provider
            .Observe<StudentGroupUpdated>()
            .Where(x => x.Id.Equals(Id))
            .ReplayEvent(ref _disposable)
            .Select(x => x.Name);
    }

    public Guid Id { get; }

    public IObservable<string> Name { get; }

    public static StudentGroup Create(Guid identifier, IEventProvider provider)
    {
        return new StudentGroup(identifier, provider);
    }

    public void Dispose()
    {
        _disposable.Dispose();
    }
}