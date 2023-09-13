using Itmo.Dev.Asap.Frontend.Application.Abstractions.Assignments.Models;

namespace Itmo.Dev.Asap.Frontend.Application.Abstractions.Assignments;

public interface IAssignmentService
{
    Task LoadAsync(Guid assignmentId, CancellationToken cancellationToken);

    Task LoadForSubjectCourseId(Guid subjectCourseId, CancellationToken cancellationToken);

    Task<CreateAssignmentResult> CreateAsync(
        Guid subjectCourseId,
        string name,
        string shortName,
        int order,
        double minPoints,
        double maxPoints,
        CancellationToken cancellationToken);

    ValueTask<UpdateMinPointsResult> UpdateMinPointsAsync(
        Guid assignmentId,
        double points,
        CancellationToken cancellationToken);

    ValueTask<UpdateMaxPointsResult> UpdateMaxPointsAsync(
        Guid assignmentId,
        double points,
        CancellationToken cancellationToken);
}