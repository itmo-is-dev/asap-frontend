using Itmo.Dev.Asap.Frontend.Application.Abstractions.Checking.Models;

namespace Itmo.Dev.Asap.Frontend.Application.Abstractions.Checking;

public interface ICheckingCodeBlocksService
{
    Task<CheckingCodeBlocksResponse> GetCheckingCodeBlocksAsync(
        long checkingId,
        Guid fistSubmissionId,
        Guid secondSubmissionId,
        int cursor,
        CancellationToken cancellationToken);
}