using Itmo.Dev.Asap.Frontend.Application.Abstractions.Identity.Events;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.Tools;
using Itmo.Dev.Asap.Frontend.Application.Events;
using Microsoft.AspNetCore.Components.Authorization;
using Phazor.Extensions;
using System.Reactive.Disposables;
using System.Security.Claims;

namespace Itmo.Dev.Asap.Frontend.Authentication;

internal class StateProvider : AuthenticationStateProvider, IEagerDependency, IDisposable
{
    private readonly IDisposable _subscription;
    private ClaimsPrincipal? _principal;
    private DateTimeOffset _lastUpdate = DateTimeOffset.MinValue;

    public StateProvider(IEventProvider provider)
    {
        _subscription = Disposable.Empty;

        provider
            .Observe<ClaimPrincipalUpdated>()
            .Subscribe(OnPrincipalUpdated)
            .CombineTo(ref _subscription);

        provider
            .Observe<AuthorizationExpired>()
            .Subscribe(OnAuthenticationExpired)
            .CombineTo(ref _subscription);
    }

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var state = new AuthenticationState(_principal ?? new ClaimsPrincipal());
        return Task.FromResult(state);
    }

    public void Dispose()
    {
        _subscription.Dispose();
    }

    private void OnPrincipalUpdated(ClaimPrincipalUpdated evt)
    {
        if (evt.Timestamp <= _lastUpdate)
            return;

        _principal = evt.Principal;
        _lastUpdate = evt.Timestamp;

        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    private void OnAuthenticationExpired(AuthorizationExpired evt)
    {
        if (evt.Timestamp <= _lastUpdate)
            return;

        _principal = null;
        _lastUpdate = evt.Timestamp;

        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
}