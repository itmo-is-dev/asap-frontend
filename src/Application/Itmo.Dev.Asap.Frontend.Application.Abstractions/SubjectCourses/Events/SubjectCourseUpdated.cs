using Itmo.Dev.Asap.Frontend.Application.Abstractions.SubjectCourses.Associations;
using Itmo.Dev.Asap.Frontend.Application.Events;

namespace Itmo.Dev.Asap.Frontend.Application.Abstractions.SubjectCourses.Events;

public record SubjectCourseUpdated(
    Guid Id,
    Guid SubjectId,
    string Name,
    IEnumerable<SubjectCourseAssociation> Associations) : IApplicationEvent;