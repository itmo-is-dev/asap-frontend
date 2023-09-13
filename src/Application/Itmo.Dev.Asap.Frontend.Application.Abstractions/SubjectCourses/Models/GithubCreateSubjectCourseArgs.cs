namespace Itmo.Dev.Asap.Frontend.Application.Abstractions.SubjectCourses.Models;

public record GithubCreateSubjectCourseArgs(long OrganizationId, long TemplateRepositoryId, long MentorTeamId);