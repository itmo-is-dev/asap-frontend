using Microsoft.AspNetCore.Components;

namespace Itmo.Dev.Asap.Frontend.Presentation.Components.Display;

public interface IDisplayContentProvider
{
    IObservable<RenderFragment?> Content { get; }
}