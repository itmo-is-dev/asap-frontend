using Itmo.Dev.Asap.Frontend.Application.Abstractions.SubjectCourses;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.SubjectCourses.Associations;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.SubjectCourses.Events;
using Itmo.Dev.Asap.Frontend.Application.Events;
using Itmo.Dev.Asap.Frontend.Application.Extensions;
using Itmo.Dev.Asap.Frontend.Application.Factories;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace Itmo.Dev.Asap.Frontend.Application.SubjectCourses;

internal class SubjectCourse : ISubjectCourse, ICreatableEntity<SubjectCourse, Guid>
{
    private readonly IDisposable _disposable;

    public SubjectCourse(Guid id, IEventProvider provider)
    {
        _disposable = Disposable.Empty;

        Id = id;

        SubjectId = provider
            .Observe<SubjectCourseUpdated>()
            .Where(x => x.Id.Equals(Id))
            .ReplayEvent(ref _disposable)
            .Select(x => x.SubjectId);

        Name = provider
            .Observe<SubjectCourseUpdated>()
            .Where(x => x.Id.Equals(Id))
            .ReplayEvent(ref _disposable)
            .Select(x => x.Name);

        Associations = provider
            .Observe<SubjectCourseUpdated>()
            .Where(x => x.Id.Equals(Id))
            .ReplayEvent(ref _disposable)
            .Select(x => x.Associations);
    }

    public Guid Id { get; }

    public IObservable<Guid> SubjectId { get; }

    public IObservable<string> Name { get; }

    public IObservable<IEnumerable<SubjectCourseAssociation>> Associations { get; }

    public static SubjectCourse Create(Guid identifier, IEventProvider provider)
    {
        return new SubjectCourse(identifier, provider);
    }

    public void Dispose()
    {
        _disposable.Dispose();
    }
}