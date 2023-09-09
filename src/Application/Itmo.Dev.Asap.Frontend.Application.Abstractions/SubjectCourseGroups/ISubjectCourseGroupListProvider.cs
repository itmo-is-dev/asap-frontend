namespace Itmo.Dev.Asap.Frontend.Application.Abstractions.SubjectCourseGroups;

public interface ISubjectCourseGroupListProvider
{
    ValueTask<ISubjectCourseGroupList> LoadAsync(Guid subjectCourseId, CancellationToken cancellationToken);
}