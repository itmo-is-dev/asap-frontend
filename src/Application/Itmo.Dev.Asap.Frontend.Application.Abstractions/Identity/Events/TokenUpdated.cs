using Itmo.Dev.Asap.Frontend.Application.Events;

namespace Itmo.Dev.Asap.Frontend.Application.Abstractions.Identity.Events;

public record TokenUpdated(string Token, DateTimeOffset Timestamp) : IApplicationEvent;