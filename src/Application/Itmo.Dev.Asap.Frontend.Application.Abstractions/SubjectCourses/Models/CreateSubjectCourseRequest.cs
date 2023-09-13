namespace Itmo.Dev.Asap.Frontend.Application.Abstractions.SubjectCourses.Models;

public record CreateSubjectCourseRequest(
    Guid SubjectId,
    string Name,
    SubjectCourseWorkflow WorkflowType,
    GithubCreateSubjectCourseArgs Args);