using Itmo.Dev.Asap.Frontend.Application.Abstractions.Subjects;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.Subjects.Events;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.Tools;
using Itmo.Dev.Asap.Frontend.Application.Events;
using Itmo.Dev.Asap.Frontend.Application.Extensions;
using Phazor.Extensions;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Itmo.Dev.Asap.Frontend.Application.Subjects;

internal class SubjectList : ISubjectList, IEagerDependency, IDisposable
{
    private readonly ISubjectService _service;
    private readonly IDisposable _disposable;
    private readonly Subject<IEnumerable<ISubject>> _subject;

    public SubjectList(ISubjectService service, IEventProvider provider)
    {
        _service = service;

        _disposable = Disposable.Empty;
        _subject = new Subject<IEnumerable<ISubject>>();

        var values = new List<ISubject>();

        provider
            .Observe<SubjectsLoaded>()
            .ReplayEvent(ref _disposable)
            .Select(x => x.Subjects)
            .Subscribe(s =>
            {
                values = s.ToList();
                _subject.OnNext(values);
            })
            .CombineTo(ref _disposable);

        provider
            .Observe<SubjectCreated>()
            .Subscribe(e =>
            {
                values.Add(e.Subject);
                _subject.OnNext(values);
            })
            .CombineTo(ref _disposable);
    }

    public IObservable<IEnumerable<ISubject>> Values => _subject;

    public async ValueTask ReloadAsync(CancellationToken cancellationToken)
    {
        await _service.LoadAsync(cancellationToken);
    }

    public void Dispose()
    {
        _disposable.Dispose();
        _subject.Dispose();
    }
}