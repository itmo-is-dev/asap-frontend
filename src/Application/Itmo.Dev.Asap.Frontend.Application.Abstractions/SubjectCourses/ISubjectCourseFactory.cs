namespace Itmo.Dev.Asap.Frontend.Application.Abstractions.SubjectCourses;

public interface ISubjectCourseFactory
{
    ISubjectCourse Create(Guid id);
}