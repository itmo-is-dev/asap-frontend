using Itmo.Dev.Asap.Frontend.Application.Events;

namespace Itmo.Dev.Asap.Frontend.Application.Abstractions.Users.Events;

public record UserUpdated(
    Guid Id,
    string FirstName,
    string MiddleName,
    string LastName,
    int? UniversityId,
    string? GithubUsername) : IApplicationEvent;