using Itmo.Dev.Asap.Frontend.Application.Abstractions.SubjectCourses;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.SubjectCourses.Events;
using Itmo.Dev.Asap.Frontend.Application.Events;
using Phazor.Extensions;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Itmo.Dev.Asap.Frontend.Application.SubjectCourses;

internal class SubjectCourseList : ISubjectCourseList
{
    private readonly IDisposable _disposable;
    private readonly ReplaySubject<IEnumerable<ISubjectCourse>> _subject;

    public SubjectCourseList(Guid subjectId, IEventProvider provider)
    {
        SubjectId = subjectId;

        _disposable = Disposable.Empty;
        _subject = new ReplaySubject<IEnumerable<ISubjectCourse>>(1);

        var values = new List<ISubjectCourse>();

        provider
            .Observe<SubjectCoursesLoaded>()
            .Where(x => x.SubjectId.Equals(SubjectId))
            .Subscribe(x =>
            {
                values = x.SubjectCourses.ToList();
                _subject.OnNext(values);
            })
            .CombineTo(ref _disposable);

        provider
            .Observe<SubjectCourseCreated>()
            .SelectMany(x => x.SubjectCourse.SubjectId, (created, s) => (created, s))
            .Where(x => x.s.Equals(subjectId))
            .Subscribe(x =>
            {
                values.Add(x.created.SubjectCourse);
                _subject.OnNext(values);
            })
            .CombineTo(ref _disposable);
    }

    public Guid SubjectId { get; }

    public IObservable<IEnumerable<ISubjectCourse>> Values => _subject;

    public void Dispose()
    {
        _disposable.Dispose();
        _subject.Dispose();
    }
}