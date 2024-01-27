namespace Itmo.Dev.Asap.Frontend.Application.Abstractions.Submissions;

public interface ISubmissionService
{
    ValueTask<bool> BanAsync(Guid studentId, Guid assignmentId, CancellationToken cancellationToken);
}