using Itmo.Dev.Asap.Frontend.Application.Abstractions.Identity.Models;

namespace Itmo.Dev.Asap.Frontend.Application.Abstractions.Identity;

public interface IIdentityService
{
    ValueTask<LoginResult> LoginAsync(string username, string password, CancellationToken cancellationToken);

    ValueTask LogoutAsync(CancellationToken cancellationToken);

    ValueTask<IReadOnlyCollection<string>> GetRolesAsync(CancellationToken cancellationToken);

    ValueTask<CreateUserAccountResult> CreateUserAccount(
        Guid id,
        string username,
        string password,
        string roleName,
        CancellationToken cancellationToken);
}