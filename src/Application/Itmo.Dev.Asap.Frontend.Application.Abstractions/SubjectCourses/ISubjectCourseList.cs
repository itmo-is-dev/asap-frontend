namespace Itmo.Dev.Asap.Frontend.Application.Abstractions.SubjectCourses;

public interface ISubjectCourseList : IDisposable
{
    Guid SubjectId { get; }

    IObservable<IEnumerable<ISubjectCourse>> Values { get; }
}