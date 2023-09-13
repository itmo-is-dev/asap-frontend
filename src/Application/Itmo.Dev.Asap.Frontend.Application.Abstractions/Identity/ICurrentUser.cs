namespace Itmo.Dev.Asap.Frontend.Application.Abstractions.Identity;

public interface ICurrentUser
{
    bool IsAuthenticated { get; }

    IObservable<IEnumerable<string>> Roles { get; }
}