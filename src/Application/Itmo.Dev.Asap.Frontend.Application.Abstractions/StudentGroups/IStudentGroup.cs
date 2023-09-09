namespace Itmo.Dev.Asap.Frontend.Application.Abstractions.StudentGroups;

public interface IStudentGroup : IDisposable
{
    Guid Id { get; }

    IObservable<string> Name { get; }
}