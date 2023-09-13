using Itmo.Dev.Asap.Frontend.Application.Abstractions.StudentGroups;

namespace Itmo.Dev.Asap.Frontend.Application.Abstractions.Students;

public interface IStudent : IDisposable
{
    Guid Id { get; }

    IObservable<string> FirstName { get; }

    IObservable<string> MiddleName { get; }

    IObservable<string> LastName { get; }

    IObservable<IStudentGroup?> Group { get; }

    IObservable<int> UniversityId { get; }

    IObservable<string?> GithubUsername { get; }
}