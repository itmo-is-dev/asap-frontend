using Itmo.Dev.Asap.Frontend.Application.Events;

namespace Itmo.Dev.Asap.Frontend.Application.Abstractions.Identity.Events;

public record AuthorizationExpired(DateTimeOffset Timestamp) : IApplicationEvent;