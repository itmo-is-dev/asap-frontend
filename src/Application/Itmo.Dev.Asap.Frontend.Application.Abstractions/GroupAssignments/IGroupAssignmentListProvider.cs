namespace Itmo.Dev.Asap.Frontend.Application.Abstractions.GroupAssignments;

public interface IGroupAssignmentListProvider
{
    Task<IGroupAssignmentList> LoadAsync(Guid assignmentId, CancellationToken cancellationToken);
}