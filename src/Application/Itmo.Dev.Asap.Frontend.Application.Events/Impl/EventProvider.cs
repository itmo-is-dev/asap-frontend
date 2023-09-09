using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Reactive.Linq;

namespace Itmo.Dev.Asap.Frontend.Application.Events.Impl;

internal class EventProvider : IEventProvider
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<EventProvider> _logger;

    public EventProvider(IServiceProvider serviceProvider, ILogger<EventProvider> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public IObservable<T> Observe<T>() where T : IApplicationEvent
    {
        IObservable<T>? observable = _serviceProvider.GetService<IObservable<T>>();

        if (observable is not null)
            return observable;

        _logger.LogWarning("No observer found for Type = {T}", typeof(T));
        return Observable.Empty<T>();
    }
}