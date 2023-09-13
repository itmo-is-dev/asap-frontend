namespace Itmo.Dev.Asap.Frontend.Application.Abstractions.Subjects.Models;

public abstract record CreateSubjectResult
{
    private CreateSubjectResult() { }

    public sealed record Success : CreateSubjectResult;

    public sealed record Failure : CreateSubjectResult;
}