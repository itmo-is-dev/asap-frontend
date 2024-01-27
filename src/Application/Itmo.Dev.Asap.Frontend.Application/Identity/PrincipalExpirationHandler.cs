using Itmo.Dev.Asap.Frontend.Application.Abstractions.Identity.Events;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.Tools;
using Itmo.Dev.Asap.Frontend.Application.Events;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Itmo.Dev.Asap.Frontend.Application.Identity;

[EagerDependencyPriority(int.MaxValue)]
internal class PrincipalExpirationHandler : IEagerDependency, IDisposable
{
    private readonly IEventPublisher _publisher;
    private readonly IDateTimeOffsetProvider _dateTimeOffsetProvider;
    private readonly IDisposable _subscription;
    private readonly ILogger<PrincipalExpirationHandler> _logger;

    private CancellationTokenSource _cts;

    public PrincipalExpirationHandler(
        IEventPublisher publisher,
        IEventProvider provider,
        IDateTimeOffsetProvider dateTimeOffsetProvider,
        ILogger<PrincipalExpirationHandler> logger)
    {
        _publisher = publisher;
        _dateTimeOffsetProvider = dateTimeOffsetProvider;
        _logger = logger;

        _cts = new CancellationTokenSource();
        _subscription = provider.Observe<ClaimPrincipalUpdated>().Subscribe(evt => _ = HandleAsync(evt));
    }

    public void Dispose()
    {
        _subscription.Dispose();
        _cts.Dispose();
    }

    private async Task HandleAsync(ClaimPrincipalUpdated evt)
    {
        _cts.Cancel();

        var cts = new CancellationTokenSource();
        _cts = cts;

        Claim? claim = evt.Principal.Claims
            .SingleOrDefault(x => x.Type.Equals(JwtRegisteredClaimNames.Exp, StringComparison.OrdinalIgnoreCase));

        if (claim is null)
        {
            _logger.LogWarning("Loaded token does not have expiration specified");
            return;
        }

        var validTo = DateTimeOffset.FromUnixTimeSeconds(long.Parse(claim.Value));
        DateTimeOffset current = _dateTimeOffsetProvider.Current;
        TimeSpan span = validTo - current;

        if (span > TimeSpan.Zero)
        {
            _logger.LogTrace("Scheduling token expiration after {Span}", span);

            // Do not use cts to avoid unhandled exceptions
            await Task.Delay(span, default);

            if (cts.IsCancellationRequested)
                return;
        }

        var expiredEvent = new AuthorizationExpired(evt.Timestamp);
        _publisher.Publish(expiredEvent);
    }
}