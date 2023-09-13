using FluentScanning;
using Itmo.Dev.Asap.Frontend.Application.Events.Impl;
using Itmo.Dev.Asap.Frontend.Application.Events.Tools;
using Microsoft.Extensions.DependencyInjection;
using System.Reactive.Subjects;

namespace Itmo.Dev.Asap.Frontend.Application.Events;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationEvents(
        this IServiceCollection collection,
        params AssemblyProvider[] assemblyProviders)
    {
        collection.AddSingleton<IEventProvider, EventProvider>();
        collection.AddSingleton<IEventPublisher, EventPublisher>();

        collection.AddOptions<EventsConfiguration>();

        var scanner = new AssemblyScanner(assemblyProviders);

        IEnumerable<Type> eventTypes = scanner
            .ScanForTypesThat()
            .AreAssignableTo<IApplicationEvent>()
            .AreNotAbstractClasses()
            .AreNotInterfaces();

        foreach (Type eventType in eventTypes)
        {
            Type subjectType = typeof(Subject<>).MakeGenericType(eventType);
            Type observableType = typeof(IObservable<>).MakeGenericType(eventType);
            Type observerType = typeof(IObserver<>).MakeGenericType(eventType);

            collection.AddSingleton(subjectType);
            collection.AddSingleton(observableType, p => p.GetRequiredService(subjectType));
            collection.AddSingleton(observerType, p => p.GetRequiredService(subjectType));
        }

        return collection;
    }

    public static IServiceCollection ConfigureApplicationEvents(
        this IServiceCollection collection,
        Action<EventsConfiguration> configuration)
    {
        return collection.Configure(configuration);
    }
}