using Itmo.Dev.Asap.Frontend.Application.Abstractions.SubjectCourses.Associations;

namespace Itmo.Dev.Asap.Frontend.Application.Abstractions.SubjectCourses;

public interface ISubjectCourse : IDisposable
{
    Guid Id { get; }

    IObservable<Guid> SubjectId { get; }

    IObservable<string> Name { get; }

    IObservable<IEnumerable<SubjectCourseAssociation>> Associations { get; }
}