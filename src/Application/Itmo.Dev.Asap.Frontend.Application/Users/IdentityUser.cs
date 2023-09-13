using Itmo.Dev.Asap.Frontend.Application.Abstractions.Users;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.Users.Events;
using Itmo.Dev.Asap.Frontend.Application.Events;
using Itmo.Dev.Asap.Frontend.Application.Extensions;
using Itmo.Dev.Asap.Frontend.Application.Factories;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace Itmo.Dev.Asap.Frontend.Application.Users;

internal class IdentityUser : IIdentityUser, ICreatableEntity<IdentityUser, Guid>
{
    private readonly IDisposable _disposable;

    public IdentityUser(Guid id, IEventProvider provider)
    {
        Id = id;

        _disposable = Disposable.Empty;

        IObservable<UserUpdated> userConnection = provider
            .Observe<UserUpdated>()
            .Where(x => x.Id.Equals(Id))
            .ReplayEvent(ref _disposable);

        IObservable<IdentityUserUpdated> identityUserConnection = provider
            .Observe<IdentityUserUpdated>()
            .Where(x => x.Id.Equals(Id))
            .ReplayEvent(ref _disposable);

        FirstName = userConnection.Select(x => x.FirstName);
        MiddleName = userConnection.Select(x => x.MiddleName);
        LastName = userConnection.Select(x => x.LastName);
        UniversityId = userConnection.Select(x => x.UniversityId);
        GithubUsername = userConnection.Select(x => x.GithubUsername);
        HasIdentity = identityUserConnection.Select(x => x.HasIdentity);
    }

    public Guid Id { get; }

    public IObservable<string> FirstName { get; }

    public IObservable<string> MiddleName { get; }

    public IObservable<string> LastName { get; }

    public IObservable<int?> UniversityId { get; }

    public IObservable<string?> GithubUsername { get; }

    public IObservable<bool> HasIdentity { get; }

    public static IdentityUser Create(Guid identifier, IEventProvider provider)
    {
        return new IdentityUser(identifier, provider);
    }

    public void Dispose()
    {
        _disposable.Dispose();
    }
}