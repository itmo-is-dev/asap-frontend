using Itmo.Dev.Asap.Frontend.Application.Abstractions.GroupAssignments.Models;

namespace Itmo.Dev.Asap.Frontend.Application.Abstractions.GroupAssignments;

public interface IGroupAssignmentService
{
    ValueTask LoadForAssignmentAsync(Guid assignmentId, CancellationToken cancellationToken);

    ValueTask<UpdateDeadlineResult> UpdateDeadlineAsync(
        Guid assignmentId,
        Guid studentGroupId,
        DateTime deadline,
        CancellationToken cancellationToken);

    ValueTask<UpdateDeadlineResult> UpdateDeadlinesAsync(
        Guid assignmentId,
        DateTime deadline,
        IEnumerable<Guid> studentGroupIds,
        CancellationToken cancellationToken);
}