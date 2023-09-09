using Blazored.LocalStorage;
using Itmo.Dev.Asap.Frontend.LocalStorage.Identity;
using Itmo.Dev.Asap.Frontend.LocalStorage.Tools;
using Microsoft.Extensions.DependencyInjection;

namespace Itmo.Dev.Asap.Frontend.LocalStorage.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddFrontendLocalStorage(this IServiceCollection collection)
    {
        collection.AddBlazoredLocalStorage();
        collection.AddSingleton<LocalStorageTokenHandler>();
        collection.AddSingleton<LocalStorageWrapper>();

        return collection;
    }
}