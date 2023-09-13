using Itmo.Dev.Asap.Frontend.Application.Abstractions.Notifications.Events;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.Users;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.Users.Events;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.Users.Models;
using Itmo.Dev.Asap.Frontend.Application.Events;
using Itmo.Dev.Asap.Frontend.Integrations.Gateway.Mapping;
using Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Models.Users;
using Itmo.Dev.Asap.Gateway.Sdk.Clients;
using Refit;

namespace Itmo.Dev.Asap.Frontend.Integrations.Gateway.Services;

internal class UserService : IUserService
{
    private readonly IUserClient _userClient;
    private readonly IIdentityUserFactory _factory;
    private readonly IEventPublisher _publisher;

    public UserService(IUserClient userClient, IIdentityUserFactory factory, IEventPublisher publisher)
    {
        _userClient = userClient;
        _factory = factory;
        _publisher = publisher;
    }

    public async Task<IEnumerable<IIdentityUser>> QueryIdentityUsersAsync(
        IdentityUserQueryModel query,
        CancellationToken cancellationToken)
    {
        var request = new QueryUserIdentityInfoRequest(
            null,
            100,
            query.NamePatterns,
            query.UniversityIds);

        var students = new List<IIdentityUser>();

        do
        {
            IApiResponse<QueryUserIdentityInfoResponse> response = await _userClient
                .QueryIdentityInfoAsync(request, cancellationToken);

            if (response.IsSuccessStatusCode is false || response.Content is null)
            {
                var errorEvent = new ErrorOccured("Failed to load students");
                _publisher.Publish(errorEvent);

                return Enumerable.Empty<IIdentityUser>();
            }

            IEnumerable<IIdentityUser> fetchedStudents = response.Content.Users
                .Select(x => _factory.Create(x.User.Id));

            students.AddRange(fetchedStudents);

            IEnumerable<UserUpdated> userEvents = response.Content.Users
                .Select(x => x.User.MapToUpdatedEvent());

            IEnumerable<IdentityUserUpdated> identityUserEvents = response.Content.Users
                .Select(x => new IdentityUserUpdated(x.User.Id, x.HasIdentity));

            _publisher.Publish(userEvents);
            _publisher.Publish(identityUserEvents);

            request = request with { PageToken = response.Content.PageToken };
        }
        while (request.PageToken is not null);

        return students;
    }
}