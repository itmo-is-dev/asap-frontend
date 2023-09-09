namespace Itmo.Dev.Asap.Frontend.Application.Abstractions.GroupAssignments;

public interface IGroupAssignment
{
    Guid AssignmentId { get; }

    Guid StudentGroupId { get; }

    IObservable<string> GroupName { get; }

    IObservable<DateTime> Deadline { get; }
}