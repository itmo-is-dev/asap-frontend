namespace Itmo.Dev.Asap.Frontend.Application.Abstractions.Students.Models;

public record StudentQueryModel(
    IEnumerable<string> NamePatterns,
    IEnumerable<string> GroupNamePatterns,
    IEnumerable<int> UniversityIds,
    IEnumerable<string> GithubUsernamePatterns);