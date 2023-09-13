namespace Itmo.Dev.Asap.Frontend.Application.Abstractions.Users;

public interface IIdentityUserFactory
{
    IIdentityUser Create(Guid id);
}