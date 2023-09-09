namespace Itmo.Dev.Asap.Frontend.Application.Abstractions.StudentGroups;

public interface IStudentGroupFactory
{
    IStudentGroup Create(Guid id);
}