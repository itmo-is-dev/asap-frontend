using Itmo.Dev.Asap.Frontend.Application.Abstractions.GroupAssignments;
using Itmo.Dev.Asap.Frontend.Application.Events;
using Itmo.Dev.Asap.Frontend.Application.Factories;

namespace Itmo.Dev.Asap.Frontend.Application.GroupAssignments;

internal class GroupAssignmentFactory : IGroupAssignmentFactory, IDisposable
{
    private readonly EntityFactory<GroupAssignment, (Guid AssignmentId, Guid StudentGroupId)> _factory;

    public GroupAssignmentFactory(IEventProvider provider)
    {
        _factory = new EntityFactory<GroupAssignment, (Guid, Guid)>(provider);
    }

    public IGroupAssignment Create(Guid assignmentId, Guid studentGroupId)
    {
        return _factory.Create((assignmentId, studentGroupId));
    }

    public void Dispose()
    {
        _factory.Dispose();
    }
}