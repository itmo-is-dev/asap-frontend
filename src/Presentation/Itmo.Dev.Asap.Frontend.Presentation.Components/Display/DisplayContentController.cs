using Microsoft.AspNetCore.Components;
using System.Reactive.Subjects;

namespace Itmo.Dev.Asap.Frontend.Presentation.Components.Display;

public class DisplayContentController : IDisplayContentProvider, IDisplayContentConsumer, IDisposable
{
    private readonly Subject<RenderFragment?> _subject;

    public DisplayContentController()
    {
        _subject = new Subject<RenderFragment?>();
    }

    public IObservable<RenderFragment?> Content => _subject;

    public void Consume(RenderFragment fragment)
    {
        _subject.OnNext(fragment);
    }

    public void Clear()
    {
        _subject.OnNext(null);
    }

    public void Dispose()
    {
        _subject.Dispose();
    }
}