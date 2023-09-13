using Itmo.Dev.Asap.Frontend.Application.Events;

namespace Itmo.Dev.Asap.Frontend.Application.Abstractions.SubjectCourses.Events;

public record SubjectCourseCreated(ISubjectCourse SubjectCourse) : IApplicationEvent;