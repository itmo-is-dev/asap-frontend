using Itmo.Dev.Asap.Frontend.Application.Abstractions.Queues.Models;
using Itmo.Dev.Asap.Frontend.Application.Events;

namespace Itmo.Dev.Asap.Frontend.Application.Abstractions.Queues.Events;

public record QueueListUpdated(Guid SubjectCourseId, IEnumerable<QueueListItem> StudentGroupIds) : IApplicationEvent;