using Itmo.Dev.Asap.Frontend.Application.Abstractions.Checking;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.Checking.Models;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.Notifications.Events;
using Itmo.Dev.Asap.Frontend.Application.Events;
using Itmo.Dev.Asap.Frontend.Integrations.Gateway.Mapping;
using Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Models.Checking;
using Itmo.Dev.Asap.Gateway.Sdk.Clients;
using Refit;

namespace Itmo.Dev.Asap.Frontend.Integrations.Gateway.Services;

internal class CheckingCodeBlocksService : ICheckingCodeBlocksService
{
    private readonly ICheckingClient _client;
    private readonly IEventPublisher _publisher;

    public CheckingCodeBlocksService(ICheckingClient client, IEventPublisher publisher)
    {
        _client = client;
        _publisher = publisher;
    }

    public async Task<CheckingCodeBlocksResponse> GetCheckingCodeBlocksAsync(
        long checkingId,
        Guid fistSubmissionId,
        Guid secondSubmissionId,
        int cursor,
        CancellationToken cancellationToken)
    {
        var request = new GetCheckingCodeBlocksRequest(
            checkingId,
            fistSubmissionId,
            secondSubmissionId,
            50,
            cursor);

        IApiResponse<GetCheckingCodeBlocksResponse> response = await _client
            .GetCheckingCodeBlocksAsync(request, cancellationToken);

        if (response.IsSuccessStatusCode is false || response.Content is null)
        {
            var errorEvent = new ErrorOccured("Failed to load code blocks");
            _publisher.Publish(errorEvent);

            return new CheckingCodeBlocksResponse(Enumerable.Empty<CheckingSimilarCodeBlocks>(), int.MaxValue, false);
        }

        IEnumerable<CheckingSimilarCodeBlocks> codeBlocks = response.Content.CodeBlocks.Select(x => x.MapToModel());

        return new CheckingCodeBlocksResponse(
            codeBlocks,
            response.Content.Count,
            response.Content.HasNext);
    }
}