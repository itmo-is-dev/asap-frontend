using Itmo.Dev.Asap.Frontend.Application.Events;

namespace Itmo.Dev.Asap.Frontend.Application.Abstractions.Assignments.Events;

public record AssignmentsLoaded(Guid SubjectCourseId, IEnumerable<IAssignment> Assignments) : IApplicationEvent;