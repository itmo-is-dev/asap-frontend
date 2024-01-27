namespace Itmo.Dev.Asap.Frontend.Application.Abstractions.Checking;

public interface ICheckingService
{
    ValueTask LoadSubjectCourseCheckingAsync(Guid subjectCourseId, CancellationToken cancellationToken);

    ValueTask StartAsync(Guid subjectCourseId, CancellationToken cancellationToken);
}