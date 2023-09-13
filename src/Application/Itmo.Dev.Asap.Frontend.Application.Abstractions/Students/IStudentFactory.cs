namespace Itmo.Dev.Asap.Frontend.Application.Abstractions.Students;

public interface IStudentFactory
{
    IStudent Create(Guid studentId);
}