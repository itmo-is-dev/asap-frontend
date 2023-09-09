using Itmo.Dev.Asap.Frontend.Application.Abstractions.Identity;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.Identity.Events;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.Tools;
using Itmo.Dev.Asap.Frontend.Application.Events;
using Phazor.Extensions;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Security.Claims;

namespace Itmo.Dev.Asap.Frontend.Application.Identity;

[EagerDependencyPriority(int.MaxValue)]
internal class CurrentUser : ICurrentUser, IEagerDependency, IDisposable
{
    private readonly IDisposable _disposable;

    public CurrentUser(IEventProvider provider)
    {
        _disposable = Disposable.Empty;

        provider
            .Observe<ClaimPrincipalUpdated>()
            .Subscribe(OnPrincipalUpdated)
            .CombineTo(ref _disposable);

        provider
            .Observe<AuthorizationExpired>()
            .Subscribe(OnAuthenticationExpired)
            .CombineTo(ref _disposable);

        IObservable<IEnumerable<string>> updatedRoles = provider
            .Observe<ClaimPrincipalUpdated>()
            .Select(x => x.Principal.Claims)
            .Select(x => x.Where(c => c.Type.Equals(ClaimTypes.Role, StringComparison.OrdinalIgnoreCase)))
            .Select(x => x.Select(c => c.Value));

        IObservable<IEnumerable<string>> expiredRoles = provider
            .Observe<AuthorizationExpired>()
            .Select(_ => Enumerable.Empty<string>());

        IConnectableObservable<IEnumerable<string>> connection = updatedRoles.Merge(expiredRoles).Replay(1);
        connection.Connect().CombineTo(ref _disposable);

        Roles = connection;
    }

    public bool IsAuthenticated { get; private set; }

    public IObservable<IEnumerable<string>> Roles { get; }

    public void Dispose()
    {
        _disposable.Dispose();
    }

    private void OnPrincipalUpdated(ClaimPrincipalUpdated evt)
    {
        IsAuthenticated = evt.Principal.Identity?.IsAuthenticated is true;
    }

    private void OnAuthenticationExpired(AuthorizationExpired evt)
    {
        IsAuthenticated = false;
    }
}