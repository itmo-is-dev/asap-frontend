namespace Itmo.Dev.Asap.Frontend.Application.Abstractions.Assignments;

public interface IAssignment
{
    Guid Id { get; }

    IObservable<Guid> SubjectCourseId { get; }

    IObservable<string> Name { get; }

    IObservable<double> MinPoints { get; }

    IObservable<double> MaxPoints { get; }
}