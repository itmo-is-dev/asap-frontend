using Itmo.Dev.Asap.Frontend.Application.Abstractions.Queues.Events;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.StudentGroups;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.Students.Events;
using Itmo.Dev.Asap.Gateway.Application.Dto.Study;
using Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Models.Queue;
using Riok.Mapperly.Abstractions;

namespace Itmo.Dev.Asap.Frontend.Integrations.Gateway.Mapping;

[Mapper(EnumMappingStrategy = EnumMappingStrategy.ByName)]
internal static partial class QueueMapper
{
    public static partial QueueSubmissionUpdated MapToEvent(this SubmissionMessage message);

    public static partial QueueSubmissionUpdated MapToEvent(this SubmissionDto submission);

    public static StudentUpdated MapToEvent(this StudentMessage message, IStudentGroupFactory factory)
    {
        return new StudentUpdated(
            message.User.Id,
            message.User.FirstName,
            message.User.MiddleName,
            message.User.LastName,
            message.GroupId is null ? null : factory.Create(message.GroupId.Value),
            message.User.UniversityId,
            message.User.GithubUsername);
    }
}