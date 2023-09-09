using Itmo.Dev.Asap.Frontend.Application.Events;
using Phazor.Extensions;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Itmo.Dev.Asap.Frontend.Application.Extensions;

internal static class ObservableExtensions
{
    public static IObservable<T> ReplayEvent<T>(this IObservable<T> observable, ref IDisposable disposable)
        where T : IApplicationEvent
    {
        IConnectableObservable<T> connection = observable.Replay(1);
        disposable = disposable.Combine(connection.Connect());

        return connection;
    }
}