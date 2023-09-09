namespace Itmo.Dev.Asap.Frontend.Application.Abstractions.Subjects;

public interface ISubjectFactory
{
    ISubject Create(Guid id);
}