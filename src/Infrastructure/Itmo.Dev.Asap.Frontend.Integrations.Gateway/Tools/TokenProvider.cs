using Itmo.Dev.Asap.Frontend.Application.Abstractions.Identity.Events;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.Tools;
using Itmo.Dev.Asap.Frontend.Application.Events;
using Itmo.Dev.Asap.Gateway.Sdk.Authentication;
using Phazor.Extensions;
using System.Reactive.Disposables;

namespace Itmo.Dev.Asap.Frontend.Integrations.Gateway.Tools;

internal class TokenProvider : ITokenProvider, IEagerDependency, IDisposable
{
    private readonly IDisposable _subscription;
    private string? _token;

    public TokenProvider(IEventProvider provider)
    {
        _subscription = Disposable.Empty;

        provider
            .Observe<TokenUpdated>()
            .Subscribe(x => _token = x.Token)
            .CombineTo(ref _subscription);

        provider
            .Observe<AuthorizationExpired>()
            .Subscribe(_ => _token = null)
            .CombineTo(ref _subscription);
    }

    public ValueTask<string?> FindTokenAsync(CancellationToken cancellationToken)
    {
        return ValueTask.FromResult(_token);
    }

    public void Dispose()
    {
        _subscription.Dispose();
    }
}