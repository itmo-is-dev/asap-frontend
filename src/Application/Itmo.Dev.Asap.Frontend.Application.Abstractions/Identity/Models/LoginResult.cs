namespace Itmo.Dev.Asap.Frontend.Application.Abstractions.Identity.Models;

public abstract record LoginResult
{
    private LoginResult() { }

    public sealed record Success : LoginResult;

    public sealed record Failure(string Message) : LoginResult;
}