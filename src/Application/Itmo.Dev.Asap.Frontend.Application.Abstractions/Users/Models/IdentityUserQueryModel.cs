namespace Itmo.Dev.Asap.Frontend.Application.Abstractions.Users.Models;

public record IdentityUserQueryModel(
    IEnumerable<string> NamePatterns,
    IEnumerable<int> UniversityIds);