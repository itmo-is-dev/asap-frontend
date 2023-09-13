using Itmo.Dev.Asap.Frontend.Application.Abstractions.Users.Models;

namespace Itmo.Dev.Asap.Frontend.Application.Abstractions.Users;

public interface IUserService
{
    Task<IEnumerable<IIdentityUser>> QueryIdentityUsersAsync(
        IdentityUserQueryModel query,
        CancellationToken cancellationToken);
}