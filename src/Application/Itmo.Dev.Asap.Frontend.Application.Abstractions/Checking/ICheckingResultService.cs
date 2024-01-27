using Itmo.Dev.Asap.Frontend.Application.Abstractions.Checking.Models;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.Models;

namespace Itmo.Dev.Asap.Frontend.Application.Abstractions.Checking;

public interface ICheckingResultService
{
    Task<CheckingResultResponse> GetCheckingResultsAsync(
        long checkingId,
        IEnumerable<Guid> assignmentIds,
        IEnumerable<Guid> groupIds,
        PageToken pageToken,
        CancellationToken cancellationToken);
}