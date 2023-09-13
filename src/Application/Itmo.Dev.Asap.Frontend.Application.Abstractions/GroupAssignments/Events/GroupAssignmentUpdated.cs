using Itmo.Dev.Asap.Frontend.Application.Events;

namespace Itmo.Dev.Asap.Frontend.Application.Abstractions.GroupAssignments.Events;

public record GroupAssignmentUpdated(
    Guid AssignmentId,
    Guid StudentGroupId,
    string GroupName,
    DateTime Deadline) : IApplicationEvent;