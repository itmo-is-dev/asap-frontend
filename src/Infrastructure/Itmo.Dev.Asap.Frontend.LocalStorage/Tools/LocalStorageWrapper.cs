using Blazored.LocalStorage;
using Microsoft.Extensions.DependencyInjection;

namespace Itmo.Dev.Asap.Frontend.LocalStorage.Tools;

public class LocalStorageWrapper
{
    private readonly IServiceScopeFactory _scopeFactory;

    public LocalStorageWrapper(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public async ValueTask<string?> GetItemAsStringAsync(string key, CancellationToken cancellationToken)
    {
        await using AsyncServiceScope scope = _scopeFactory.CreateAsyncScope();
        ILocalStorageService localStorage = scope.ServiceProvider.GetRequiredService<ILocalStorageService>();

        return await localStorage.GetItemAsStringAsync(key, cancellationToken);
    }

    public async ValueTask SetItemAsStringAsync(string key, string value, CancellationToken cancellationToken)
    {
        await using AsyncServiceScope scope = _scopeFactory.CreateAsyncScope();
        ILocalStorageService localStorage = scope.ServiceProvider.GetRequiredService<ILocalStorageService>();

        await localStorage.SetItemAsStringAsync(key, value, cancellationToken);
    }

    public async ValueTask RemoveItemAsync(string key, CancellationToken cancellationToken)
    {
        await using AsyncServiceScope scope = _scopeFactory.CreateAsyncScope();
        ILocalStorageService localStorage = scope.ServiceProvider.GetRequiredService<ILocalStorageService>();

        await localStorage.RemoveItemAsync(key, cancellationToken);
    }
}