namespace Itmo.Dev.Asap.Frontend.Application.Abstractions.Subjects;

public interface ISubject
{
    Guid Id { get; }

    IObservable<string> Name { get; }
}