namespace Itmo.Dev.Asap.Frontend.Application.Abstractions.GroupAssignments.Models;

public abstract record UpdateDeadlineResult
{
    private UpdateDeadlineResult() { }

    public sealed record Success : UpdateDeadlineResult;

    public sealed record Failure : UpdateDeadlineResult;
}