using Itmo.Dev.Asap.Frontend.Application.Abstractions.Submissions;
using Itmo.Dev.Asap.Frontend.Application.Events;

namespace Itmo.Dev.Asap.Frontend.Application.Abstractions.Queues.Events;

public record QueueSubmissionUpdated(
    Guid Id,
    Guid StudentId,
    DateTime? SubmissionDate,
    string Payload,
    string AssignmentShortName,
    SubmissionState State) : IApplicationEvent;