using Itmo.Dev.Asap.Frontend.Application.Events;

namespace Itmo.Dev.Asap.Frontend.Application.Abstractions.GroupAssignments.Events;

public record GroupAssignmentsLoaded(
    Guid AssignmentId,
    IEnumerable<IGroupAssignment> GroupAssignments) : IApplicationEvent;