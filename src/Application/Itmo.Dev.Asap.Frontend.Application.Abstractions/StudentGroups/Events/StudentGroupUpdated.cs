using Itmo.Dev.Asap.Frontend.Application.Events;

namespace Itmo.Dev.Asap.Frontend.Application.Abstractions.StudentGroups.Events;

public record StudentGroupUpdated(Guid Id, string Name) : IApplicationEvent;