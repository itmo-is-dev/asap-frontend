using Itmo.Dev.Asap.Frontend.Application.Events;

namespace Itmo.Dev.Asap.Frontend.Application.Abstractions.Assignments.Events;

public record AssignmentUpdated(
    Guid Id,
    Guid SubjectCourseId,
    string Name,
    double MinPoints,
    double MaxPoints) : IApplicationEvent;