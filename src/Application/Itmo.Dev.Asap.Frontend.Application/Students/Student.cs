using Itmo.Dev.Asap.Frontend.Application.Abstractions.StudentGroups;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.Students;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.Students.Events;
using Itmo.Dev.Asap.Frontend.Application.Events;
using Itmo.Dev.Asap.Frontend.Application.Extensions;
using Itmo.Dev.Asap.Frontend.Application.Factories;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace Itmo.Dev.Asap.Frontend.Application.Students;

public class Student : IStudent, ICreatableEntity<Student, Guid>
{
    private readonly IDisposable _disposable;

    public Student(Guid id, IEventProvider provider)
    {
        Id = id;

        _disposable = Disposable.Empty;

        IObservable<StudentUpdated> connection = provider
            .Observe<StudentUpdated>()
            .Where(x => x.Id.Equals(Id))
            .ReplayEvent(ref _disposable);

        FirstName = connection.Select(x => x.FirstName);
        MiddleName = connection.Select(x => x.MiddleName);
        LastName = connection.Select(x => x.LastName);
        Group = connection.Select(x => x.Group);
        GithubUsername = connection.Select(x => x.GithubUsername);

        UniversityId = connection
            .Select(x => x.UniversityId)
            .Where(x => x is not null)
            .Select(x => x!.Value);
    }

    public Guid Id { get; }

    public IObservable<string> FirstName { get; }

    public IObservable<string> MiddleName { get; }

    public IObservable<string> LastName { get; }

    public IObservable<IStudentGroup?> Group { get; }

    public IObservable<int> UniversityId { get; }

    public IObservable<string?> GithubUsername { get; }

    public static Student Create(Guid identifier, IEventProvider provider)
    {
        return new Student(identifier, provider);
    }

    public void Dispose()
    {
        _disposable.Dispose();
    }
}