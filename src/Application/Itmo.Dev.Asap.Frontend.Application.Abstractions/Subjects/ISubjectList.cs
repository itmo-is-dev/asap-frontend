namespace Itmo.Dev.Asap.Frontend.Application.Abstractions.Subjects;

public interface ISubjectList
{
    IObservable<IEnumerable<ISubject>> Values { get; }

    ValueTask ReloadAsync(CancellationToken cancellationToken);
}