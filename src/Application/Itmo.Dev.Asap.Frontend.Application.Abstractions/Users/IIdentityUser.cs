namespace Itmo.Dev.Asap.Frontend.Application.Abstractions.Users;

public interface IIdentityUser : IDisposable
{
    Guid Id { get; }

    IObservable<string> FirstName { get; }

    IObservable<string> MiddleName { get; }

    IObservable<string> LastName { get; }

    IObservable<int?> UniversityId { get; }

    IObservable<string?> GithubUsername { get; }

    IObservable<bool> HasIdentity { get; }
}