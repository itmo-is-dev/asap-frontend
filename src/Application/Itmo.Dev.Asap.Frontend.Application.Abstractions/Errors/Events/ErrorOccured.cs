using Itmo.Dev.Asap.Frontend.Application.Events;

namespace Itmo.Dev.Asap.Frontend.Application.Abstractions.Errors.Events;

public record ErrorOccured(string Message) : IApplicationEvent;