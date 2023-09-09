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

    public StateProvider(IEventProvider provider)
    {
        _subscription = Disposable.Empty;

        provider
            .Observe<ClaimPrincipalUpdated>()
            .Subscribe(OnPrincipalUpdated)
            .CombineTo(ref _subscription);

        provider
            .Observe<AuthorizationExpired>()
            .Subscribe(_ => OnAuthenticationExpired())
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
        _principal = evt.Principal;
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    private void OnAuthenticationExpired()
    {
        _principal = null;
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
}