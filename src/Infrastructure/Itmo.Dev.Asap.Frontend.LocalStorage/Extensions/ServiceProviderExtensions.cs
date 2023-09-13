using Itmo.Dev.Asap.Frontend.LocalStorage.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Itmo.Dev.Asap.Frontend.LocalStorage.Extensions;

public static class ServiceProviderExtensions
{
    public static async ValueTask UseFrontendLocalStorageAsync(this IServiceProvider provider)
    {
        LocalStorageTokenHandler handler = provider.GetRequiredService<LocalStorageTokenHandler>();
        await handler.LoadAsync();
    }
}