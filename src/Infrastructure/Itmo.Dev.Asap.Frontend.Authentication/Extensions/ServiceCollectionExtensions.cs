using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Itmo.Dev.Asap.Frontend.Authentication.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddFrontendAuthentication(this IServiceCollection collection)
    {
        collection.AddSingleton<StateProvider>();
        collection.AddScoped<AuthenticationStateProvider>(p => p.GetRequiredService<StateProvider>());

        collection.AddAuthorizationCore();

        return collection;
    }
}