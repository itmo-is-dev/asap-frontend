namespace Itmo.Dev.Asap.Frontend.Application.Abstractions.SubjectCourses.Associations;

public record GithubSubjectCourseAssociation(
    Guid SubjectCourseId,
    long OrganizationId,
    string OrganizationName,
    long TemplateRepositoryId,
    string TemplateRepositoryName,
    long MentorTeamId,
    string MentorTeamName) : SubjectCourseAssociation(SubjectCourseId);