using Itmo.Dev.Asap.Frontend.Application.Events;

namespace Itmo.Dev.Asap.Frontend.Application.Abstractions.Checking.Events;

public record SubjectCourseHasActiveCheckingUpdated(Guid SubjectCourseId, bool HasActive) : IApplicationEvent;