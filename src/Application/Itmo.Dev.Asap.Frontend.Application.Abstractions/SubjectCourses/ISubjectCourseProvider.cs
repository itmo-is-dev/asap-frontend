namespace Itmo.Dev.Asap.Frontend.Application.Abstractions.SubjectCourses;

public interface ISubjectCourseProvider
{
    Task<ISubjectCourse> LoadAsync(Guid subjectCourseId, CancellationToken cancellationToken);
}