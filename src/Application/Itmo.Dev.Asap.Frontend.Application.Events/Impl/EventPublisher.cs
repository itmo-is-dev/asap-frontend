using Itmo.Dev.Asap.Frontend.Application.Events.Tools;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Itmo.Dev.Asap.Frontend.Application.Events.Impl;

internal class EventPublisher : IEventPublisher
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<EventPublisher> _logger;
    private readonly EventsConfiguration _configuration;

    public EventPublisher(
        IServiceProvider serviceProvider,
        ILogger<EventPublisher> logger,
        IOptions<EventsConfiguration> configuration)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _configuration = configuration.Value;
    }

    public void Publish<T>(T evt) where T : IApplicationEvent
    {
        IObserver<T>? observer = _serviceProvider.GetService<IObserver<T>>();

        if (observer is null)
        {
            _logger.LogWarning("No observer found for event of Type = {T}", typeof(T));
        }
        else
        {
            if (_configuration.EnablePublishLogging)
            {
                _logger.LogInformation("Publishing event = {Event}", evt);
            }

            observer.OnNext(evt);
        }
    }

    public void Publish<T>(IEnumerable<T> events) where T : IApplicationEvent
    {
        IObserver<T>? observer = _serviceProvider.GetService<IObserver<T>>();

        if (observer is null)
        {
            _logger.LogWarning("No observer found for event of Type = {T}", typeof(T));
        }
        else
        {
            foreach (T evt in events)
            {
                if (_configuration.EnablePublishLogging)
                {
                    _logger.LogInformation("Publishing event = {Event}", evt);
                }

                observer.OnNext(evt);
            }
        }
    }
}