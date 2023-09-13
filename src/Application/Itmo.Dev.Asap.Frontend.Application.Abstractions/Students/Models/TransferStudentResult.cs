namespace Itmo.Dev.Asap.Frontend.Application.Abstractions.Students.Models;

public abstract record TransferStudentResult
{
    private TransferStudentResult() { }

    public sealed record Success : TransferStudentResult;

    public sealed record Failure : TransferStudentResult;
}