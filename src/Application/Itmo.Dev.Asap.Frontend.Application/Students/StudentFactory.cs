using Itmo.Dev.Asap.Frontend.Application.Abstractions.Students;
using Itmo.Dev.Asap.Frontend.Application.Events;
using Itmo.Dev.Asap.Frontend.Application.Factories;

namespace Itmo.Dev.Asap.Frontend.Application.Students;

public class StudentFactory : IStudentFactory, IDisposable
{
    private readonly EntityFactory<Student, Guid> _factory;

    public StudentFactory(IEventProvider provider)
    {
        _factory = new EntityFactory<Student, Guid>(provider);
    }

    public IStudent Create(Guid studentId)
    {
        return _factory.Create(studentId);
    }

    public void Dispose()
    {
        _factory.Dispose();
    }
}