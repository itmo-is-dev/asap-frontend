using Itmo.Dev.Asap.Frontend.Application.Abstractions.Subjects;
using Itmo.Dev.Asap.Frontend.Application.Events;
using Itmo.Dev.Asap.Frontend.Application.Factories;

namespace Itmo.Dev.Asap.Frontend.Application.Subjects;

internal class SubjectFactory : ISubjectFactory, IDisposable
{
    private readonly EntityFactory<Subject, Guid> _factory;

    public SubjectFactory(IEventProvider provider)
    {
        _factory = new EntityFactory<Subject, Guid>(provider);
    }

    public ISubject Create(Guid id)
    {
        return _factory.Create(id);
    }

    public void Dispose()
    {
        _factory.Dispose();
    }
}