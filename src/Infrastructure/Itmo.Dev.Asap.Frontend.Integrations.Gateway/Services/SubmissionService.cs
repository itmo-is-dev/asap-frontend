using Itmo.Dev.Asap.Frontend.Application.Abstractions.Notifications.Events;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.Submissions;
using Itmo.Dev.Asap.Frontend.Application.Events;
using Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Models.Submissions;
using Itmo.Dev.Asap.Gateway.Sdk.Clients;
using Refit;

namespace Itmo.Dev.Asap.Frontend.Integrations.Gateway.Services;

public class SubmissionService : ISubmissionService
{
    private readonly ISubmissionsClient _client;
    private readonly IEventPublisher _publisher;

    public SubmissionService(ISubmissionsClient client, IEventPublisher publisher)
    {
        _client = client;
        _publisher = publisher;
    }

    public async ValueTask<bool> BanAsync(Guid studentId, Guid assignmentId, CancellationToken cancellationToken)
    {
        var request = new BanSubmissionRequest(studentId, assignmentId);
        IApiResponse response = await _client.BanAsync(request, cancellationToken);

        if (response.IsSuccessStatusCode is false)
        {
            var errorEvent = new ErrorOccured("Failed to ban submission");
            _publisher.Publish(errorEvent);

            return false;
        }

        var successEvent = new SuccessfulOperationOccured("Submission banned successfully");
        _publisher.Publish(successEvent);

        return true;
    }
}