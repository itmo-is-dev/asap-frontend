namespace Itmo.Dev.Asap.Frontend.Application.Abstractions.Assignments;

public interface IAssignmentFactory
{
    IAssignment Create(Guid id);
}