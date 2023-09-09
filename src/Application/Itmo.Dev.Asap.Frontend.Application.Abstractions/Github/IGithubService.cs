using Itmo.Dev.Asap.Frontend.Application.Abstractions.Github.Models;

namespace Itmo.Dev.Asap.Frontend.Application.Abstractions.Github;

public interface IGithubService
{
    ValueTask SyncMentorsAsync(long organizationId, CancellationToken cancellationToken);

    ValueTask SyncOrganizationAsync(Guid subjectCourseId, CancellationToken cancellationToken);

    Task<IEnumerable<GithubOrganization>> SearchOrganizationsAsync(string query, CancellationToken cancellationToken);

    Task<IEnumerable<GithubRepository>> SearchRepositoriesAsync(
        long organizationId,
        string query,
        CancellationToken cancellationToken);

    Task<IEnumerable<GithubTeam>> SearchTeamsAsync(
        long organizationId,
        string query,
        CancellationToken cancellationToken);
}