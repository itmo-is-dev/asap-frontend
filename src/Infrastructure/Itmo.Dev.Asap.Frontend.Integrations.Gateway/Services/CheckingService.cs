using Itmo.Dev.Asap.Frontend.Application.Abstractions.Checking;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.Checking.Events;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.Checking.Models;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.Notifications.Events;
using Itmo.Dev.Asap.Frontend.Application.Events;
using Itmo.Dev.Asap.Frontend.Integrations.Gateway.Mapping;
using Itmo.Dev.Asap.Gateway.Application.Dto.Checking;
using Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Models;
using Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Models.Checking;
using Itmo.Dev.Asap.Gateway.Sdk.Clients;
using Itmo.Dev.Asap.Gateway.Sdk.Extensions;
using Refit;

namespace Itmo.Dev.Asap.Frontend.Integrations.Gateway.Services;

internal class CheckingService : ICheckingService
{
    private readonly ICheckingClient _client;
    private readonly IEventPublisher _publisher;

    public CheckingService(ICheckingClient client, IEventPublisher publisher)
    {
        _client = client;
        _publisher = publisher;
    }

    public async ValueTask LoadSubjectCourseCheckingAsync(Guid subjectCourseId, CancellationToken cancellationToken)
    {
        var request = new GetCheckingsRequest(subjectCourseId, int.MaxValue, null);
        IApiResponse<GetCheckingsResponse> response = await _client.GetCheckingsAsync(request, cancellationToken);

        if (response.IsSuccessStatusCode is false || response.Content is null)
        {
            var errorEvent = new ErrorOccured("Failed to load checkings");
            _publisher.Publish(errorEvent);

            return;
        }

        bool hasActive = response.Content.Checkings.Any(x => x.IsCompleted is false);
        var hasActiveEvent = new SubjectCourseHasActiveCheckingUpdated(subjectCourseId, hasActive);

        _publisher.Publish(hasActiveEvent);

        IEnumerable<SubjectCourseChecking> checkings = response.Content.Checkings.Select(x => x.MapToModel());
        var checkingsEvent = new SubjectCourseCheckingsLoaded(subjectCourseId, checkings);

        _publisher.Publish(checkingsEvent);
    }

    public async ValueTask StartAsync(Guid subjectCourseId, CancellationToken cancellationToken)
    {
        var request = new StartCheckingRequest(subjectCourseId);
        IApiResponse<CheckingDto> response = await _client.StartAsync(request, cancellationToken);

        if (response.IsSuccessStatusCode is false || response.Content is null)
        {
            ErrorDetails? details = await response.TryGetErrorDetailsAsync();

            var errorEvent = new ErrorOccured(details?.Message ?? "Failed to start checking");
            _publisher.Publish(errorEvent);

            return;
        }

        var hasActiveEvent = new SubjectCourseHasActiveCheckingUpdated(subjectCourseId, true);
        _publisher.Publish(hasActiveEvent);

        SubjectCourseChecking checking = response.Content.MapToModel();
        var checkingsEvent = new SubjectCourseCheckingsLoaded(subjectCourseId, new[] { checking });

        _publisher.Publish(checkingsEvent);
    }
}