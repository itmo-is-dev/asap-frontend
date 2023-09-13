namespace Itmo.Dev.Asap.Frontend.Application.Abstractions.Assignments;

public interface IAssignmentList : IDisposable
{
    Guid SubjectCourseId { get; }

    IObservable<IEnumerable<IAssignment>> Values { get; }
}