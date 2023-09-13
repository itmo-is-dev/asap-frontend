using Itmo.Dev.Asap.Frontend.Application.Abstractions.GroupAssignments;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.GroupAssignments.Events;
using Itmo.Dev.Asap.Frontend.Application.Events;
using Itmo.Dev.Asap.Frontend.Application.Extensions;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace Itmo.Dev.Asap.Frontend.Application.GroupAssignments;

internal class GroupAssignmentList : IGroupAssignmentList
{
    private readonly IDisposable _disposable;

    public GroupAssignmentList(Guid assignmentId, IEventProvider provider)
    {
        AssignmentId = assignmentId;

        _disposable = Disposable.Empty;

        IObservable<GroupAssignmentsLoaded> connection = provider
            .Observe<GroupAssignmentsLoaded>()
            .Where(x => x.AssignmentId.Equals(AssignmentId))
            .ReplayEvent(ref _disposable);

        Values = connection.Select(x => x.GroupAssignments);
    }

    public Guid AssignmentId { get; }

    public IObservable<IEnumerable<IGroupAssignment>> Values { get; }

    public void Dispose()
    {
        _disposable.Dispose();
    }
}