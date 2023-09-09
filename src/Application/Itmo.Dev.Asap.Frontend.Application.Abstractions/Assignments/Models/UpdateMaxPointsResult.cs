namespace Itmo.Dev.Asap.Frontend.Application.Abstractions.Assignments.Models;

public abstract record UpdateMaxPointsResult
{
    private UpdateMaxPointsResult() { }

    public sealed record Success : UpdateMaxPointsResult;

    public sealed record Failure : UpdateMaxPointsResult;
}