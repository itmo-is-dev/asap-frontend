using Itmo.Dev.Asap.Frontend.Application.Abstractions.StudentGroups;
using Itmo.Dev.Asap.Frontend.Application.Events;
using Itmo.Dev.Asap.Frontend.Application.Factories;

namespace Itmo.Dev.Asap.Frontend.Application.StudentGroups;

internal class StudentGroupFactory : IStudentGroupFactory, IDisposable
{
    private readonly EntityFactory<StudentGroup, Guid> _factory;

    public StudentGroupFactory(IEventProvider provider)
    {
        _factory = new EntityFactory<StudentGroup, Guid>(provider);
    }

    public IStudentGroup Create(Guid id)
    {
        return _factory.Create(id);
    }

    public void Dispose()
    {
        _factory.Dispose();
    }
}