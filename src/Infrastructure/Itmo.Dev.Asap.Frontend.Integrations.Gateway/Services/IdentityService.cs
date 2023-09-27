using Itmo.Dev.Asap.Frontend.Application.Abstractions.Identity;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.Identity.Events;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.Identity.Models;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.Notifications.Events;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.Users.Events;
using Itmo.Dev.Asap.Frontend.Application.Events;
using Itmo.Dev.Asap.Frontend.Integrations.Gateway.Mapping;
using Itmo.Dev.Asap.Gateway.Application.Dto.Users;
using Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Models;
using Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Models.Identity;
using Itmo.Dev.Asap.Gateway.Sdk.Clients;
using Itmo.Dev.Asap.Gateway.Sdk.Extensions;
using Microsoft.Extensions.Logging;
using Refit;

namespace Itmo.Dev.Asap.Frontend.Integrations.Gateway.Services;

internal class IdentityService : IIdentityService
{
    private readonly IIdentityClient _client;
    private readonly IEventPublisher _publisher;
    private readonly ILogger<IdentityService> _logger;

    public IdentityService(IIdentityClient client, IEventPublisher publisher, ILogger<IdentityService> logger)
    {
        _client = client;
        _publisher = publisher;
        _logger = logger;
    }

    public async ValueTask<LoginResult> LoginAsync(
        string username,
        string password,
        CancellationToken cancellationToken)
    {
        try
        {
            var request = new LoginRequest(username, password);
            IApiResponse<LoginResponse> response = await _client.LoginAsync(request, cancellationToken);

            Console.WriteLine($"Login response code: {response.StatusCode}");

            if (response.IsSuccessStatusCode is false || response.Content is null)
            {
                ErrorDetails? error = await response.TryGetErrorDetailsAsync();

                return new LoginResult.Failure(error?.Message ?? string.Empty);
            }

            var evt = new TokenUpdated(response.Content.Token);
            _publisher.Publish(evt);

            return new LoginResult.Success();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to execute login");
            return new LoginResult.Failure("Failed to send request");
        }
    }

    public ValueTask LogoutAsync(CancellationToken cancellationToken)
    {
        var evt = new AuthorizationExpired();
        _publisher.Publish(evt);

        return ValueTask.CompletedTask;
    }

    public async ValueTask<IReadOnlyCollection<string>> GetRolesAsync(CancellationToken cancellationToken)
    {
        IApiResponse<IReadOnlyCollection<string>> response = await _client.GetRolesAsync(cancellationToken);

        if (response is { IsSuccessStatusCode: true, Content: not null })
            return response.Content;

        ErrorDetails? details = await response.TryGetErrorDetailsAsync();

        var errorEvent = new ErrorOccured(details?.Message ?? "Failed to load user roles");
        _publisher.Publish(errorEvent);

        return Array.Empty<string>();
    }

    public async ValueTask<CreateUserAccountResult> CreateUserAccount(
        Guid id,
        string username,
        string password,
        string roleName,
        CancellationToken cancellationToken)
    {
        var request = new CreateUserAccountRequest(username, password, roleName);

        IApiResponse<UserIdentityInfoDto> response = await _client
            .CreateUserAccountAsync(id, request, cancellationToken);

        if (response.IsSuccessStatusCode is false || response.Content is null)
        {
            ErrorDetails? details = await response.TryGetErrorDetailsAsync();

            var errorEvent = new ErrorOccured(details?.Message ?? "Failed to create identity account");
            _publisher.Publish(errorEvent);

            return new CreateUserAccountResult.Failure();
        }

        UserUpdated userEvent = response.Content.User.MapToUpdatedEvent();
        var identityUserEvent = new IdentityUserUpdated(response.Content.User.Id, response.Content.HasIdentity);

        _publisher.Publish(userEvent);
        _publisher.Publish(identityUserEvent);

        return new CreateUserAccountResult.Success();
    }
}