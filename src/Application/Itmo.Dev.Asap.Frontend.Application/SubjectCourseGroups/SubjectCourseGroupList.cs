using Itmo.Dev.Asap.Frontend.Application.Abstractions.SubjectCourseGroups;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.SubjectCourseGroups.Events;
using Itmo.Dev.Asap.Frontend.Application.Events;
using Phazor.Extensions;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Itmo.Dev.Asap.Frontend.Application.SubjectCourseGroups;

internal class SubjectCourseGroupList : ISubjectCourseGroupList
{
    private readonly IDisposable _disposable;
    private readonly ReplaySubject<IEnumerable<ISubjectCourseGroup>> _subject;

    public SubjectCourseGroupList(Guid subjectCourseId, IEventProvider provider)
    {
        SubjectCourseId = subjectCourseId;

        _disposable = Disposable.Empty;
        _subject = new ReplaySubject<IEnumerable<ISubjectCourseGroup>>();

        var values = new List<ISubjectCourseGroup>();

        provider
            .Observe<SubjectCourseGroupsLoaded>()
            .Where(x => x.SubjectCourseId.Equals(SubjectCourseId))
            .Subscribe(e =>
            {
                values = e.Groups.ToList();
                _subject.OnNext(values);
            })
            .CombineTo(ref _disposable);

        provider
            .Observe<SubjectCourseGroupsCreated>()
            .Where(x => x.SubjectCourseId.Equals(SubjectCourseId))
            .Subscribe(e =>
            {
                values.AddRange(e.Groups);
                _subject.OnNext(values);
            })
            .CombineTo(ref _disposable);
    }

    public Guid SubjectCourseId { get; }

    public IObservable<IEnumerable<ISubjectCourseGroup>> Values => _subject;

    public void Dispose()
    {
        _disposable.Dispose();
        _subject.Dispose();
    }
}