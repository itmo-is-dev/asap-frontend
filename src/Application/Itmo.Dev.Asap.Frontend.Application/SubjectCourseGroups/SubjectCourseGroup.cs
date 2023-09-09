using Itmo.Dev.Asap.Frontend.Application.Abstractions.SubjectCourseGroups;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.SubjectCourseGroups.Events;
using Itmo.Dev.Asap.Frontend.Application.Events;
using Itmo.Dev.Asap.Frontend.Application.Extensions;
using Itmo.Dev.Asap.Frontend.Application.Factories;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace Itmo.Dev.Asap.Frontend.Application.SubjectCourseGroups;

internal class SubjectCourseGroup :
    ISubjectCourseGroup,
    ICreatableEntity<SubjectCourseGroup, (Guid SubjectCourseId, Guid StudentGroupId)>
{
    private readonly IDisposable _disposable;

    public SubjectCourseGroup(Guid subjectCourseId, Guid studentGroupId, IEventProvider provider)
    {
        SubjectCourseId = subjectCourseId;
        StudentGroupId = studentGroupId;

        _disposable = Disposable.Empty;

        IObservable<SubjectCourseGroupUpdated> connection = provider
            .Observe<SubjectCourseGroupUpdated>()
            .Where(x => x.SubjectCourseId.Equals(SubjectCourseId) && x.StudentGroupId.Equals(StudentGroupId))
            .ReplayEvent(ref _disposable);

        GroupName = connection.Select(x => x.GroupName);
    }

    public Guid SubjectCourseId { get; }

    public Guid StudentGroupId { get; }

    public IObservable<string> GroupName { get; }

    public static SubjectCourseGroup Create(
        (Guid SubjectCourseId, Guid StudentGroupId) identifier,
        IEventProvider provider)
    {
        return new SubjectCourseGroup(identifier.SubjectCourseId, identifier.StudentGroupId, provider);
    }

    public void Dispose()
    {
        _disposable.Dispose();
    }
}