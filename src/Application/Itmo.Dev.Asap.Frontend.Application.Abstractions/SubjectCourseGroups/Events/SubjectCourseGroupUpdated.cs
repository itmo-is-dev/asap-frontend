using Itmo.Dev.Asap.Frontend.Application.Events;

namespace Itmo.Dev.Asap.Frontend.Application.Abstractions.SubjectCourseGroups.Events;

public record SubjectCourseGroupUpdated(
    Guid SubjectCourseId,
    Guid StudentGroupId,
    string GroupName) : IApplicationEvent;