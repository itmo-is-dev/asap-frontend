namespace Itmo.Dev.Asap.Frontend.Application.Abstractions.SubjectCourses;

public interface ISubjectCourseListProvider
{
    Task<ISubjectCourseList> LoadAsync(Guid subjectId, CancellationToken cancellationToken);
}