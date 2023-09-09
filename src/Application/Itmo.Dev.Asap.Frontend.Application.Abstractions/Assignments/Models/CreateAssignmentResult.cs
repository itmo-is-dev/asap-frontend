namespace Itmo.Dev.Asap.Frontend.Application.Abstractions.Assignments.Models;

public abstract record CreateAssignmentResult
{
    private CreateAssignmentResult() { }

    public sealed record Success : CreateAssignmentResult;

    public sealed record Failure : CreateAssignmentResult;
}