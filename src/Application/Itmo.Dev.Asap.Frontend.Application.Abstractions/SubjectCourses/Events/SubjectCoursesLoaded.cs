using Itmo.Dev.Asap.Frontend.Application.Events;

namespace Itmo.Dev.Asap.Frontend.Application.Abstractions.SubjectCourses.Events;

public record SubjectCoursesLoaded(Guid SubjectId, IEnumerable<ISubjectCourse> SubjectCourses) : IApplicationEvent;