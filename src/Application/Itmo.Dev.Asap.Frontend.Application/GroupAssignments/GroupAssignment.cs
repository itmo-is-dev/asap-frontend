using Itmo.Dev.Asap.Frontend.Application.Abstractions.GroupAssignments;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.GroupAssignments.Events;
using Itmo.Dev.Asap.Frontend.Application.Events;
using Itmo.Dev.Asap.Frontend.Application.Extensions;
using Itmo.Dev.Asap.Frontend.Application.Factories;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace Itmo.Dev.Asap.Frontend.Application.GroupAssignments;

internal class GroupAssignment :
    IGroupAssignment,
    ICreatableEntity<GroupAssignment, (Guid AssignmentId, Guid StudentGroupId)>
{
    private readonly IDisposable _disposable;

    public GroupAssignment(Guid assignmentId, Guid studentGroupId, IEventProvider provider)
    {
        AssignmentId = assignmentId;
        StudentGroupId = studentGroupId;

        _disposable = Disposable.Empty;

        IObservable<GroupAssignmentUpdated> connection = provider
            .Observe<GroupAssignmentUpdated>()
            .Where(x => x.AssignmentId.Equals(AssignmentId) && x.StudentGroupId.Equals(StudentGroupId))
            .ReplayEvent(ref _disposable);

        GroupName = connection.Select(x => x.GroupName);
        Deadline = connection.Select(x => x.Deadline);
    }

    public Guid AssignmentId { get; }

    public Guid StudentGroupId { get; }

    public IObservable<string> GroupName { get; }

    public IObservable<DateTime> Deadline { get; }

    public static GroupAssignment Create(
        (Guid AssignmentId, Guid StudentGroupId) identifier,
        IEventProvider provider)
    {
        return new GroupAssignment(identifier.AssignmentId, identifier.StudentGroupId, provider);
    }

    public void Dispose()
    {
        _disposable.Dispose();
    }
}