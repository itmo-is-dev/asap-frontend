namespace Itmo.Dev.Asap.Frontend.Application.Abstractions.Assignments.Models;

public abstract record UpdateMinPointsResult
{
    private UpdateMinPointsResult() { }

    public sealed record Success : UpdateMinPointsResult;

    public sealed record Failure : UpdateMinPointsResult;
}