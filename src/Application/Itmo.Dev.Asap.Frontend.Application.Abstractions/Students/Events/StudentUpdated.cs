using Itmo.Dev.Asap.Frontend.Application.Abstractions.StudentGroups;
using Itmo.Dev.Asap.Frontend.Application.Events;

namespace Itmo.Dev.Asap.Frontend.Application.Abstractions.Students.Events;

public record StudentUpdated(
    Guid Id,
    string FirstName,
    string MiddleName,
    string LastName,
    IStudentGroup? Group,
    int? UniversityId,
    string? GithubUsername) : IApplicationEvent;