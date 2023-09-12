using Itmo.Dev.Asap.Frontend.Application.Events;

namespace Itmo.Dev.Asap.Frontend.Application.Abstractions.Notifications.Events;

public record ErrorOccured(string Message) : IApplicationEvent;