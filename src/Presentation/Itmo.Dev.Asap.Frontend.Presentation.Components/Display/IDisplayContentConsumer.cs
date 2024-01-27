using Microsoft.AspNetCore.Components;

namespace Itmo.Dev.Asap.Frontend.Presentation.Components.Display;

public interface IDisplayContentConsumer
{
    void Consume(RenderFragment fragment);

    void Clear();
}