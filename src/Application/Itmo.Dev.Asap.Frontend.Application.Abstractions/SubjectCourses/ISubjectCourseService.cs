using Itmo.Dev.Asap.Frontend.Application.Abstractions.SubjectCourses.Models;

namespace Itmo.Dev.Asap.Frontend.Application.Abstractions.SubjectCourses;

public interface ISubjectCourseService
{
    ValueTask LoadForSubjectAsync(Guid subjectId, CancellationToken cancellationToken);

    ValueTask LoadAsync(Guid subjectCourseId, CancellationToken cancellationToken);

    ValueTask SyncPointsAsync(Guid subjectCourseId, CancellationToken cancellationToken);

    ValueTask<CreateSubjectCourseResult> CreateAsync(
        CreateSubjectCourseRequest request,
        CancellationToken cancellationToken);
}