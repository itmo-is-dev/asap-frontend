using Itmo.Dev.Asap.Frontend.Application.Abstractions.Assignments;
using Itmo.Dev.Asap.Frontend.Application.Events;
using Itmo.Dev.Asap.Frontend.Application.Factories;

namespace Itmo.Dev.Asap.Frontend.Application.Assignments;

internal class AssignmentFactory : IAssignmentFactory, IDisposable
{
    private readonly EntityFactory<Assignment, Guid> _factory;

    public AssignmentFactory(IEventProvider provider)
    {
        _factory = new EntityFactory<Assignment, Guid>(provider);
    }

    public IAssignment Create(Guid id)
    {
        return _factory.Create(id);
    }

    public void Dispose()
    {
        _factory.Dispose();
    }
}