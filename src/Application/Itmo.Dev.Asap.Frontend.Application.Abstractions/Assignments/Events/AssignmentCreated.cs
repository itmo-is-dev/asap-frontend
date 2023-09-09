using Itmo.Dev.Asap.Frontend.Application.Events;

namespace Itmo.Dev.Asap.Frontend.Application.Abstractions.Assignments.Events;

public record AssignmentCreated(IAssignment Assignment) : IApplicationEvent;