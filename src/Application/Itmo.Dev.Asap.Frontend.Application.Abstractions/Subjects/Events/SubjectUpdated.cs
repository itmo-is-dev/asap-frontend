using Itmo.Dev.Asap.Frontend.Application.Events;

namespace Itmo.Dev.Asap.Frontend.Application.Abstractions.Subjects.Events;

public record SubjectUpdated(Guid Id, string Name) : IApplicationEvent;