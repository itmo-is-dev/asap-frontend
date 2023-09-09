using Itmo.Dev.Asap.Frontend.Application.Abstractions.Users;
using Itmo.Dev.Asap.Frontend.Application.Events;
using Itmo.Dev.Asap.Frontend.Application.Factories;

namespace Itmo.Dev.Asap.Frontend.Application.Users;

public class IdentityUserFactory : IIdentityUserFactory, IDisposable
{
    private readonly EntityFactory<IdentityUser, Guid> _factory;

    public IdentityUserFactory(IEventProvider provider)
    {
        _factory = new EntityFactory<IdentityUser, Guid>(provider);
    }

    public IIdentityUser Create(Guid id)
    {
        return _factory.Create(id);
    }

    public void Dispose()
    {
        _factory.Dispose();
    }
}