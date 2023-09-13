using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;
using Phazor.Components.Extensions;

namespace Itmo.Dev.Asap.Frontend.Presentation.Markup.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMarkup(this IServiceCollection collection, bool enablePhazorTracing)
    {
        collection.AddMudServices();
        collection.AddPhazorComponents(x => x.Trace = enablePhazorTracing);

        return collection;
    }
}