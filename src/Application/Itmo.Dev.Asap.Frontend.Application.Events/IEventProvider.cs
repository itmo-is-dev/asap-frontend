namespace Itmo.Dev.Asap.Frontend.Application.Events;

public interface IEventProvider
{
    IObservable<T> Observe<T>() where T : IApplicationEvent;
}