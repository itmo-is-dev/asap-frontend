using Itmo.Dev.Asap.Frontend.Application.Abstractions.Students.Models;

namespace Itmo.Dev.Asap.Frontend.Application.Abstractions.Students;

public interface IStudentService
{
    Task<IEnumerable<IStudent>> QueryStudentsAsync(
        StudentQueryModel query,
        CancellationToken cancellationToken);

    Task<TransferStudentResult> TransferStudentAsync(
        Guid studentId,
        Guid? studentGroupId,
        CancellationToken cancellationToken);
}