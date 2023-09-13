using Itmo.Dev.Asap.Frontend.Application.Abstractions.StudentGroups.Models;

namespace Itmo.Dev.Asap.Frontend.Application.Abstractions.StudentGroups;

public interface IStudentGroupService
{
    Task<IEnumerable<IStudentGroup>> QueryGroupsAsync(
        StudentGroupQueryModel query,
        CancellationToken cancellationToken);
}