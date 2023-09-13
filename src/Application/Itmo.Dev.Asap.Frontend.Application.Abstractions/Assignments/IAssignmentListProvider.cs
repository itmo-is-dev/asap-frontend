namespace Itmo.Dev.Asap.Frontend.Application.Abstractions.Assignments;

public interface IAssignmentListProvider
{
    Task<IAssignmentList> LoadAsync(Guid subjectCourseId, CancellationToken cancellationToken);
}