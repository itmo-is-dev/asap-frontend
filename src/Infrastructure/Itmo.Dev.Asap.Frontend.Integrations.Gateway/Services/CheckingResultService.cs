using Itmo.Dev.Asap.Frontend.Application.Abstractions.Checking;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.Checking.Models;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.Models;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.Notifications.Events;
using Itmo.Dev.Asap.Frontend.Application.Events;
using Itmo.Dev.Asap.Frontend.Integrations.Gateway.Mapping;
using Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Models.Checking;
using Itmo.Dev.Asap.Gateway.Sdk.Clients;
using Refit;

namespace Itmo.Dev.Asap.Frontend.Integrations.Gateway.Services;

internal class CheckingResultService : ICheckingResultService
{
    private readonly ICheckingClient _client;
    private readonly IEventPublisher _publisher;

    public CheckingResultService(ICheckingClient client, IEventPublisher publisher)
    {
        _client = client;
        _publisher = publisher;
    }

    public async Task<CheckingResultResponse> GetCheckingResultsAsync(
        long checkingId,
        IEnumerable<Guid> assignmentIds,
        IEnumerable<Guid> groupIds,
        PageToken pageToken,
        CancellationToken cancellationToken)
    {
        var request = new GetCheckingResultsRequest(checkingId, assignmentIds, groupIds, 20, pageToken.Value);

        IApiResponse<GetCheckingResultsResponse> response = await _client
            .GetCheckingResultsAsync(request, cancellationToken);

        if (response.IsSuccessStatusCode is false || response.Content is null)
        {
            var errorEvent = new ErrorOccured("Failed to load checking results");
            _publisher.Publish(errorEvent);

            return new CheckingResultResponse(Enumerable.Empty<SubjectCourseCheckingResult>(), new PageToken(null));
        }

        IEnumerable<SubjectCourseCheckingResult> results = response.Content.Results.Select(x => x.MapToModel());

        return new CheckingResultResponse(
            results,
            new PageToken(response.Content.PageToken));
    }
}