namespace Itmo.Dev.Asap.Frontend.Application.Abstractions.SubjectCourseGroups.Models;

public abstract record CreateSubjectCourseGroupResult
{
    private CreateSubjectCourseGroupResult() { }

    public sealed record Success : CreateSubjectCourseGroupResult;

    public sealed record Failure : CreateSubjectCourseGroupResult;
}