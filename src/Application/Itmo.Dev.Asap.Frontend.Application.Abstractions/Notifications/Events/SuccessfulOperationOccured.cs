using Itmo.Dev.Asap.Frontend.Application.Events;

namespace Itmo.Dev.Asap.Frontend.Application.Abstractions.Notifications.Events;

public record SuccessfulOperationOccured(string Message) : IApplicationEvent;