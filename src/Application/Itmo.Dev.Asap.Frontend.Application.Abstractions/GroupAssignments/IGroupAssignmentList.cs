namespace Itmo.Dev.Asap.Frontend.Application.Abstractions.GroupAssignments;

public interface IGroupAssignmentList : IDisposable
{
    Guid AssignmentId { get; }

    IObservable<IEnumerable<IGroupAssignment>> Values { get; }
}