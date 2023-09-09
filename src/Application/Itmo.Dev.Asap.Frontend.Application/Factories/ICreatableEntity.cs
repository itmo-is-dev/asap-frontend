using Itmo.Dev.Asap.Frontend.Application.Events;

namespace Itmo.Dev.Asap.Frontend.Application.Factories;

public interface ICreatableEntity<out TEntity, in TIdentifier> : IDisposable
    where TEntity : IDisposable
{
    static abstract TEntity Create(TIdentifier identifier, IEventProvider provider);
}