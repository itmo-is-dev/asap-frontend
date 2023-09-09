using Itmo.Dev.Asap.Frontend.Application.Abstractions.StudentGroups;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.Students.Events;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.Users.Events;
using Itmo.Dev.Asap.Gateway.Application.Dto.Users;
using Riok.Mapperly.Abstractions;

namespace Itmo.Dev.Asap.Frontend.Integrations.Gateway.Mapping;

[Mapper]
public static partial class EventMapper
{
    public static StudentUpdated MapToUpdatedEvent(this StudentDto student, IStudentGroupFactory studentGroupFactory)
    {
        return new StudentUpdated(
            student.User.Id,
            student.User.FirstName,
            student.User.MiddleName,
            student.User.LastName,
            student.GroupId is null ? null : studentGroupFactory.Create(student.GroupId.Value),
            student.User.UniversityId,
            student.User.GithubUsername);
    }

    public static partial UserUpdated MapToUpdatedEvent(this UserDto user);
}