using Itmo.Dev.Asap.Frontend.Application.Events;

namespace Itmo.Dev.Asap.Frontend.Application.Factories;

internal class EntityFactory<TEntity, TIdentifier> : IDisposable
    where TEntity : class, ICreatableEntity<TEntity, TIdentifier>
    where TIdentifier : notnull
{
    private readonly IEventProvider _provider;
    private readonly Dictionary<TIdentifier, WeakReference<TEntity>> _subjects;

    public EntityFactory(IEventProvider provider)
    {
        _provider = provider;
        _subjects = new Dictionary<TIdentifier, WeakReference<TEntity>>();
    }

    public TEntity Create(TIdentifier identifier)
    {
        if (_subjects.TryGetValue(identifier, out WeakReference<TEntity>? reference)
            && reference.TryGetTarget(out TEntity? entity))
        {
            return entity;
        }

        entity = TEntity.Create(identifier, _provider);
        _subjects[identifier] = new WeakReference<TEntity>(entity);

        return entity;
    }

    public void Dispose()
    {
        foreach (WeakReference<TEntity> reference in _subjects.Values)
        {
            if (reference.TryGetTarget(out TEntity? subject))
                subject.Dispose();
        }
    }
}