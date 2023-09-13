namespace Itmo.Dev.Asap.Frontend.Application.Abstractions.Identity.Models;

public abstract record CreateUserAccountResult
{
    private CreateUserAccountResult() { }

    public sealed record Success : CreateUserAccountResult;

    public sealed record Failure : CreateUserAccountResult;
}