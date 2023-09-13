using Itmo.Dev.Asap.Frontend.Application.Abstractions.Subjects.Models;

namespace Itmo.Dev.Asap.Frontend.Application.Abstractions.Subjects;

public interface ISubjectService
{
    Task LoadAsync(CancellationToken cancellationToken);

    Task LoadAsync(Guid id, CancellationToken cancellationToken);

    Task<CreateSubjectResult> CreateAsync(string name, CancellationToken cancellationToken);
}