using Itmo.Dev.Asap.Frontend.Application.Abstractions.Checking;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.Checking.Events;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.Checking.Models;
using Itmo.Dev.Asap.Frontend.Application.Events;
using Itmo.Dev.Asap.Frontend.Application.Extensions;
using Phazor.Extensions;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Itmo.Dev.Asap.Frontend.Application.Checking;

internal class CheckingList : ICheckingList
{
    private readonly IDisposable _disposable;
    private readonly ReplaySubject<IEnumerable<SubjectCourseChecking>> _subject;

    public CheckingList(Guid subjectCourseId, IEventProvider provider)
    {
        SubjectCourseId = subjectCourseId;

        _disposable = Disposable.Empty;
        _subject = new ReplaySubject<IEnumerable<SubjectCourseChecking>>();

        var values = new HashSet<SubjectCourseChecking>(SubjectCourseCheckingComparer.Instance);

        provider
            .Observe<SubjectCourseCheckingsLoaded>()
            .Where(x => x.SubjectCourseId.Equals(subjectCourseId))
            .Subscribe(e =>
            {
                foreach (SubjectCourseChecking checking in e.Checkings)
                {
                    values.Add(checking);
                }

                _subject.OnNext(values);
            })
            .CombineTo(ref _disposable);

        HasActive = provider
            .Observe<SubjectCourseHasActiveCheckingUpdated>()
            .Where(x => x.SubjectCourseId.Equals(subjectCourseId))
            .ReplayEvent(ref _disposable)
            .Select(x => x.HasActive);
    }

    public Guid SubjectCourseId { get; }

    public IObservable<bool> HasActive { get; }

    public IObservable<IEnumerable<SubjectCourseChecking>> Values => _subject;

    public void Dispose()
    {
        _disposable.Dispose();
        _subject.Dispose();
    }
}