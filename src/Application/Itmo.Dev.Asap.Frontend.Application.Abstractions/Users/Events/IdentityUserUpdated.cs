using Itmo.Dev.Asap.Frontend.Application.Events;

namespace Itmo.Dev.Asap.Frontend.Application.Abstractions.Users.Events;

public record IdentityUserUpdated(Guid Id, bool HasIdentity) : IApplicationEvent;