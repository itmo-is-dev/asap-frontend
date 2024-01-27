using Itmo.Dev.Asap.Frontend.Application.Abstractions.Identity.Events;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.Tools;
using Itmo.Dev.Asap.Frontend.Application.Events;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Itmo.Dev.Asap.Frontend.Application.Identity;

internal class PrincipalParserTokenHandler : IEagerDependency, IDisposable
{
    private readonly IEventPublisher _publisher;
    private readonly IDisposable _subscription;

    public PrincipalParserTokenHandler(IEventPublisher publisher, IEventProvider provider)
    {
        _publisher = publisher;

        _subscription = provider.Observe<TokenUpdated>().Subscribe(Handle);
    }

    public void Dispose()
    {
        _subscription.Dispose();
    }

    private void Handle(TokenUpdated evt)
    {
        var handler = new JwtSecurityTokenHandler();
        JwtSecurityToken token = handler.ReadJwtToken(evt.Token);

        var identity = new ClaimsIdentity(token.Claims, "jwt");
        var principal = new ClaimsPrincipal(identity);

        var principalEvent = new ClaimPrincipalUpdated(principal, evt.Timestamp);
        _publisher.Publish(principalEvent);
    }
}