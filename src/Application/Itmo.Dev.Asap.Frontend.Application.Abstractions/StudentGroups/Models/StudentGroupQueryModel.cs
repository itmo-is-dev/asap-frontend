namespace Itmo.Dev.Asap.Frontend.Application.Abstractions.StudentGroups.Models;

public record StudentGroupQueryModel(
    IEnumerable<Guid> ExcludedIds,
    IEnumerable<string> NamePatterns,
    IEnumerable<Guid> SubjectCourseIds,
    IEnumerable<Guid> ExcludedSubjectCourseIds);