using Itmo.Dev.Asap.Frontend.Application.Abstractions.Notifications.Events;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.StudentGroups;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.StudentGroups.Events;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.Students;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.Students.Events;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.Students.Models;
using Itmo.Dev.Asap.Frontend.Application.Events;
using Itmo.Dev.Asap.Frontend.Integrations.Gateway.Mapping;
using Itmo.Dev.Asap.Gateway.Application.Dto.Users;
using Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Models;
using Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Models.Students;
using Itmo.Dev.Asap.Gateway.Sdk.Clients;
using Itmo.Dev.Asap.Gateway.Sdk.Extensions;
using Refit;

namespace Itmo.Dev.Asap.Frontend.Integrations.Gateway.Services;

internal class StudentService : IStudentService
{
    private readonly IStudentClient _client;
    private readonly IEventPublisher _publisher;
    private readonly IStudentFactory _studentFactory;
    private readonly IStudentGroupFactory _studentGroupFactory;

    public StudentService(
        IStudentClient client,
        IEventPublisher publisher,
        IStudentFactory studentFactory,
        IStudentGroupFactory studentGroupFactory)
    {
        _client = client;
        _publisher = publisher;
        _studentFactory = studentFactory;
        _studentGroupFactory = studentGroupFactory;
    }

    public async Task<IEnumerable<IStudent>> QueryStudentsAsync(
        StudentQueryModel query,
        CancellationToken cancellationToken)
    {
        var request = new QueryStudentRequest(
            null,
            100,
            query.NamePatterns,
            query.GroupNamePatterns,
            query.UniversityIds,
            query.GithubUsernamePatterns);

        var students = new List<IStudent>();

        var groups = new Dictionary<Guid, StudentDto>();

        do
        {
            IApiResponse<QueryStudentResponse> response = await _client.QueryAsync(request, cancellationToken);

            if (response.IsSuccessStatusCode is false || response.Content is null)
            {
                var errorEvent = new ErrorOccured("Failed to load students");
                _publisher.Publish(errorEvent);

                return Enumerable.Empty<IStudent>();
            }

            IEnumerable<IStudent> fetchedStudents = response.Content.Students
                .Select(x => _studentFactory.Create(x.User.Id));

            students.AddRange(fetchedStudents);

            IEnumerable<StudentUpdated> events = response.Content.Students
                .Select(x => x.MapToUpdatedEvent(_studentGroupFactory));

            _publisher.Publish(events);

            foreach (StudentDto student in response.Content.Students)
            {
                if (student.GroupId is null)
                    continue;

                groups[student.GroupId.Value] = student;
            }

            request = request with { PageToken = response.Content.PageToken };
        }
        while (request.PageToken is not null);

        IEnumerable<StudentGroupUpdated> groupEvents = groups.Values
            .Select(x => new StudentGroupUpdated(x.GroupId!.Value, x.GroupName));

        _publisher.Publish(groupEvents);

        return students;
    }

    public async Task<TransferStudentResult> TransferStudentAsync(
        Guid studentId,
        Guid? studentGroupId,
        CancellationToken cancellationToken)
    {
        if (studentGroupId is null)
            return await DismissStudentAsync(studentId, cancellationToken);

        var request = new TransferStudentRequest(studentGroupId.Value);
        IApiResponse<StudentDto> response = await _client.TransferToGroupAsync(studentId, request, cancellationToken);

        if (response.IsSuccessStatusCode is false || response.Content is null)
        {
            ErrorDetails? details = await response.TryGetErrorDetailsAsync();

            var errorEvent = new ErrorOccured(details?.Message ?? "Failed to update deadline");
            _publisher.Publish(errorEvent);

            return new TransferStudentResult.Failure();
        }

        StudentUpdated studentEvent = response.Content.MapToUpdatedEvent(_studentGroupFactory);
        _publisher.Publish(studentEvent);

        if (response.Content.GroupId is not null)
        {
            var groupEvent = new StudentGroupUpdated(response.Content.GroupId.Value, response.Content.GroupName);
            _publisher.Publish(groupEvent);
        }

        return new TransferStudentResult.Success();
    }

    private async Task<TransferStudentResult> DismissStudentAsync(Guid studentId, CancellationToken cancellationToken)
    {
        IApiResponse<StudentDto> response = await _client.DismissFromGroupAsync(studentId, cancellationToken);

        if (response is { IsSuccessStatusCode: true, Content: not null })
        {
            StudentUpdated userEvent = response.Content.MapToUpdatedEvent(_studentGroupFactory);
            _publisher.Publish(userEvent);

            return new TransferStudentResult.Success();
        }

        ErrorDetails? details = await response.TryGetErrorDetailsAsync();

        var errorEvent = new ErrorOccured(details?.Message ?? "Failed to dismiss ");
        _publisher.Publish(errorEvent);

        return new TransferStudentResult.Failure();
    }
}