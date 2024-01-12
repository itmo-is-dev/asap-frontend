namespace Itmo.Dev.Asap.Frontend.Application.Abstractions.Checking;

public interface ICheckingListProvider
{
    Task<ICheckingList> GetCheckingListAsync(Guid subjectCourseId, CancellationToken cancellationToken);
}