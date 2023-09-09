using Itmo.Dev.Asap.Frontend.Application.Abstractions.Assignments;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.Assignments.Events;
using Itmo.Dev.Asap.Frontend.Application.Events;
using Phazor.Extensions;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Itmo.Dev.Asap.Frontend.Application.Assignments;

internal class AssignmentList : IAssignmentList
{
    private readonly IDisposable _disposable;
    private readonly ReplaySubject<IEnumerable<IAssignment>> _subject;

    public AssignmentList(Guid subjectCourseId, IEventProvider provider)
    {
        SubjectCourseId = subjectCourseId;

        _disposable = Disposable.Empty;
        _subject = new ReplaySubject<IEnumerable<IAssignment>>();

        var values = new List<IAssignment>();

        provider
            .Observe<AssignmentsLoaded>()
            .Where(x => x.SubjectCourseId.Equals(SubjectCourseId))
            .Subscribe(x =>
            {
                values = x.Assignments.ToList();
                _subject.OnNext(values);
            })
            .CombineTo(ref _disposable);

        provider
            .Observe<AssignmentCreated>()
            .SelectMany(x => x.Assignment.SubjectCourseId, (e, id) => (e, id))
            .Where(x => x.id.Equals(SubjectCourseId))
            .Subscribe(x =>
            {
                values.Add(x.e.Assignment);
                _subject.OnNext(values);
            })
            .CombineTo(ref _disposable);
    }

    public Guid SubjectCourseId { get; }

    public IObservable<IEnumerable<IAssignment>> Values => _subject;

    public void Dispose()
    {
        _disposable.Dispose();
        _subject.Dispose();
    }
}