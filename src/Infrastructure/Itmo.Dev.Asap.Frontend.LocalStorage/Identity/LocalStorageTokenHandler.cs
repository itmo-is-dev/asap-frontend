using Itmo.Dev.Asap.Frontend.Application.Abstractions.Identity.Events;
using Itmo.Dev.Asap.Frontend.Application.Events;
using Itmo.Dev.Asap.Frontend.LocalStorage.Tools;
using Phazor.Extensions;
using System.Reactive.Disposables;

namespace Itmo.Dev.Asap.Frontend.LocalStorage.Identity;

internal class LocalStorageTokenHandler : IDisposable
{
    public const string TokenKey = "asap.access_token";

    private readonly IEventPublisher _publisher;
    private readonly LocalStorageWrapper _localStorage;
    private readonly IDisposable _subscription;

    /// <summary>
    ///     Caching events emitted by handler to avoid redundant token updates
    /// </summary>
    private readonly HashSet<TokenUpdated> _eventCache;

    public LocalStorageTokenHandler(
        IEventProvider provider,
        LocalStorageWrapper localStorage,
        IEventPublisher publisher)
    {
        _localStorage = localStorage;
        _publisher = publisher;

        _eventCache = new HashSet<TokenUpdated>();

        _subscription = Disposable.Empty;

        provider
            .Observe<TokenUpdated>()
            .Subscribe(token => _ = SaveTokenAsync(token))
            .CombineTo(ref _subscription);

        provider
            .Observe<AuthorizationExpired>()
            .Subscribe(evt => _ = RemoveTokenAsync())
            .CombineTo(ref _subscription);
    }

    public async ValueTask LoadAsync()
    {
        string? token = await _localStorage.GetItemAsStringAsync(TokenKey, default);

        if (token is null)
            return;

        var evt = new TokenUpdated(token, DateTimeOffset.UtcNow);
        _eventCache.Add(evt);

        _publisher.Publish(evt);
    }

    public void Dispose()
    {
        _subscription.Dispose();
    }

    private async Task SaveTokenAsync(TokenUpdated evt)
    {
        // If event was emitted by handler – skip it's processing
        if (_eventCache.Remove(evt))
            return;

        await _localStorage.SetItemAsStringAsync(TokenKey, evt.Token, default);
    }

    private async Task RemoveTokenAsync()
    {
        await _localStorage.RemoveItemAsync(TokenKey, default);
    }
}