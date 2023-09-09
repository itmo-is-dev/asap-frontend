namespace Itmo.Dev.Asap.Frontend.Application.Abstractions.Subjects;

public interface ISubjectProvider
{
    Task<ISubject> LoadAsync(Guid subjectId, CancellationToken cancellationToken);
}