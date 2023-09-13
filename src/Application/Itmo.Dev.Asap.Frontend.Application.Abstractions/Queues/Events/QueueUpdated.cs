using Itmo.Dev.Asap.Frontend.Application.Events;

namespace Itmo.Dev.Asap.Frontend.Application.Abstractions.Queues.Events;

public record QueueUpdated(
    Guid SubjectCourseId,
    Guid StudentGroupId,
    IEnumerable<IQueueSubmission> Submissions) : IApplicationEvent;