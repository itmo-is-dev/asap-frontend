namespace Itmo.Dev.Asap.Frontend.Application.Abstractions.SubjectCourseGroups;

public interface ISubjectCourseGroup
{
    Guid SubjectCourseId { get; }

    Guid StudentGroupId { get; }

    IObservable<string> GroupName { get; }
}