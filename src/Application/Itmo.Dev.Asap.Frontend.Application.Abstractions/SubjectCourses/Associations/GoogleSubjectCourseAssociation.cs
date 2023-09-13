namespace Itmo.Dev.Asap.Frontend.Application.Abstractions.SubjectCourses.Associations;

public record GoogleSubjectCourseAssociation(
    Guid SubjectCourseId,
    string SpreadsheetId,
    string SpreadsheetName) : SubjectCourseAssociation(SubjectCourseId);