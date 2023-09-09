namespace Itmo.Dev.Asap.Frontend.Application.Abstractions.SubjectCourses.Models;

public abstract record CreateSubjectCourseResult
{
    private CreateSubjectCourseResult() { }

    public sealed record Success : CreateSubjectCourseResult;

    public sealed record Failure : CreateSubjectCourseResult;
}