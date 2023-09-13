using Itmo.Dev.Asap.Frontend.Application.Events;

namespace Itmo.Dev.Asap.Frontend.Application.Abstractions.SubjectCourseGroups.Events;

public record SubjectCourseGroupsLoaded(
    Guid SubjectCourseId,
    IEnumerable<ISubjectCourseGroup> Groups) : IApplicationEvent;