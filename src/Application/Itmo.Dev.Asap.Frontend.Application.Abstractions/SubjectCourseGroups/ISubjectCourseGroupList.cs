namespace Itmo.Dev.Asap.Frontend.Application.Abstractions.SubjectCourseGroups;

public interface ISubjectCourseGroupList : IDisposable
{
    Guid SubjectCourseId { get; }

    IObservable<IEnumerable<ISubjectCourseGroup>> Values { get; }
}