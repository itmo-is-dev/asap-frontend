using Itmo.Dev.Asap.Frontend.Application.Abstractions.SubjectCourseGroups.Models;

namespace Itmo.Dev.Asap.Frontend.Application.Abstractions.SubjectCourseGroups;

public interface ISubjectCourseGroupService
{
    ValueTask LoadForSubjectCourseAsync(Guid subjectCourseId, CancellationToken cancellationToken);

    ValueTask<CreateSubjectCourseGroupResult> CreateAsync(
        Guid subjectCourseId,
        IEnumerable<Guid> studentGroupIds,
        CancellationToken cancellationToken);

    ValueTask<IEnumerable<SubjectCourseGroupInfo>> QueryAsync(
        Guid subjectCourseId,
        IEnumerable<string> names,
        CancellationToken cancellationToken);
}