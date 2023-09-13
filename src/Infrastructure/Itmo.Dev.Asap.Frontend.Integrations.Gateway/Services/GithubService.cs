using Itmo.Dev.Asap.Frontend.Application.Abstractions.Github;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.Github.Models;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.Notifications.Events;
using Itmo.Dev.Asap.Frontend.Application.Events;
using Itmo.Dev.Asap.Gateway.Application.Dto.Github;
using Itmo.Dev.Asap.Gateway.Sdk.Clients;
using Refit;

namespace Itmo.Dev.Asap.Frontend.Integrations.Gateway.Services;

internal class GithubService : IGithubService
{
    private readonly IGithubManagementClient _client;
    private readonly IGithubSearchClient _searchClient;
    private readonly IEventPublisher _publisher;

    public GithubService(IGithubManagementClient client, IEventPublisher publisher, IGithubSearchClient searchClient)
    {
        _client = client;
        _publisher = publisher;
        _searchClient = searchClient;
    }

    public async ValueTask SyncMentorsAsync(long organizationId, CancellationToken cancellationToken)
    {
        IApiResponse response = await _client.ForceMentorSyncAsync(organizationId, cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            var evt = new SuccessfulOperationOccured("Successfully synchronized mentors");
            _publisher.Publish(evt);
        }
        else
        {
            var evt = new ErrorOccured("Failed to sync mentors");
            _publisher.Publish(evt);
        }
    }

    public async ValueTask SyncOrganizationAsync(Guid subjectCourseId, CancellationToken cancellationToken)
    {
        IApiResponse response = await _client.ForceOrganizationUpdateAsync(subjectCourseId, cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            var evt = new SuccessfulOperationOccured("Successfully synchronized organization");
            _publisher.Publish(evt);
        }
        else
        {
            var evt = new ErrorOccured("Failed to sync organization");
        }
    }

    public async Task<IEnumerable<GithubOrganization>> SearchOrganizationsAsync(
        string query,
        CancellationToken cancellationToken)
    {
        IApiResponse<IReadOnlyCollection<GithubOrganizationDto>> response = await _searchClient
            .SearchOrganizationsAsync(query, cancellationToken);

        if (response is { IsSuccessStatusCode: true, Content: not null })
            return response.Content.Select(x => new GithubOrganization(x.Id, x.Name, x.AvatarUrl));

        var errorEvent = new ErrorOccured("Failed to search organizations");
        _publisher.Publish(errorEvent);

        return Enumerable.Empty<GithubOrganization>();
    }

    public async Task<IEnumerable<GithubRepository>> SearchRepositoriesAsync(
        long organizationId,
        string query,
        CancellationToken cancellationToken)
    {
        IApiResponse<IReadOnlyCollection<GithubRepositoryDto>> response = await _searchClient
            .SearchRepositoriesAsync(organizationId, query, cancellationToken);

        if (response is { IsSuccessStatusCode: true, Content: not null })
            return response.Content.Select(x => new GithubRepository(x.Id, x.Name));

        var errorEvent = new ErrorOccured("Failed to search repositories");
        _publisher.Publish(errorEvent);

        return Enumerable.Empty<GithubRepository>();
    }

    public async Task<IEnumerable<GithubTeam>> SearchTeamsAsync(
        long organizationId,
        string query,
        CancellationToken cancellationToken)
    {
        IApiResponse<IReadOnlyCollection<GithubTeamDto>> response = await _searchClient
            .SearchTeamsAsync(organizationId, query, cancellationToken);

        if (response is { IsSuccessStatusCode: true, Content: not null })
            return response.Content.Select(x => new GithubTeam(x.Id, x.Name));

        var errorEvent = new ErrorOccured("Failed to search teams");
        _publisher.Publish(errorEvent);

        return Enumerable.Empty<GithubTeam>();
    }
}