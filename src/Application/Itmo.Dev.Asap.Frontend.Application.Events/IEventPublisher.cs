namespace Itmo.Dev.Asap.Frontend.Application.Events;

public interface IEventPublisher
{
    void Publish<T>(T evt) where T : IApplicationEvent;

    void Publish<T>(IEnumerable<T> events) where T : IApplicationEvent;
}