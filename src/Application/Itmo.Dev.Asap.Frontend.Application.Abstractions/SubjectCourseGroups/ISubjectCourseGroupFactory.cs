namespace Itmo.Dev.Asap.Frontend.Application.Abstractions.SubjectCourseGroups;

public interface ISubjectCourseGroupFactory
{
    ISubjectCourseGroup Create(Guid subjectCourseId, Guid studentGroupId);
}