using Itmo.Dev.Asap.Frontend.Application.Abstractions.Notifications.Events;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.StudentGroups;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.StudentGroups.Events;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.StudentGroups.Models;
using Itmo.Dev.Asap.Frontend.Application.Events;
using Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Models.StudentGroups;
using Itmo.Dev.Asap.Gateway.Sdk.Clients;
using Refit;

namespace Itmo.Dev.Asap.Frontend.Integrations.Gateway.Services;

internal class StudentGroupService : IStudentGroupService
{
    private readonly IStudentGroupClient _client;
    private readonly IStudentGroupFactory _factory;
    private readonly IEventPublisher _publisher;

    public StudentGroupService(IStudentGroupClient client, IStudentGroupFactory factory, IEventPublisher publisher)
    {
        _client = client;
        _factory = factory;
        _publisher = publisher;
    }

    public async Task<IEnumerable<IStudentGroup>> QueryGroupsAsync(
        StudentGroupQueryModel query,
        CancellationToken cancellationToken)
    {
        var request = new QueryStudentGroupRequest(
            null,
            100,
            query.ExcludedIds,
            query.NamePatterns,
            query.SubjectCourseIds,
            query.ExcludedSubjectCourseIds);

        var studentGroups = new List<IStudentGroup>();

        do
        {
            IApiResponse<QueryStudentGroupResponse> response = await _client.QueryAsync(request, cancellationToken);

            if (response.IsSuccessStatusCode is false || response.Content is null)
            {
                var errorEvent = new ErrorOccured("Failed to load student groups");
                _publisher.Publish(errorEvent);

                return Enumerable.Empty<IStudentGroup>();
            }

            IEnumerable<IStudentGroup> fetchedGroups = response.Content.StudentGroups
                .Select(x => _factory.Create(x.Id));

            studentGroups.AddRange(fetchedGroups);

            IEnumerable<StudentGroupUpdated> events = response.Content.StudentGroups
                .Select(x => new StudentGroupUpdated(x.Id, x.Name));

            _publisher.Publish(events);

            request = request with { PageToken = response.Content.PageToken };
        }
        while (request.PageToken is not null);

        return studentGroups;
    }
}