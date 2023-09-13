namespace Itmo.Dev.Asap.Frontend.Application.Abstractions.GroupAssignments;

public interface IGroupAssignmentFactory
{
    IGroupAssignment Create(Guid assignmentId, Guid studentGroupId);
}