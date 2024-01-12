using Itmo.Dev.Asap.Frontend.Application.Abstractions.Checking.Models;

namespace Itmo.Dev.Asap.Frontend.Application.Abstractions.Checking;

public interface ICheckingList : IDisposable
{
    Guid SubjectCourseId { get; }

    IObservable<bool> HasActive { get; }

    IObservable<IEnumerable<SubjectCourseChecking>> Values { get; }
}