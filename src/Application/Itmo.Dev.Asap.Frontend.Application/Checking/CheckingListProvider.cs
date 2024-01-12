using Itmo.Dev.Asap.Frontend.Application.Abstractions.Checking;
using Itmo.Dev.Asap.Frontend.Application.Events;

namespace Itmo.Dev.Asap.Frontend.Application.Checking;

internal class CheckingListProvider : ICheckingListProvider
{
    private readonly IEventProvider _provider;
    private readonly ICheckingService _checkingService;

    public CheckingListProvider(IEventProvider provider, ICheckingService checkingService)
    {
        _provider = provider;
        _checkingService = checkingService;
    }

    public async Task<ICheckingList> GetCheckingListAsync(Guid subjectCourseId, CancellationToken cancellationToken)
    {
        var list = new CheckingList(subjectCourseId, _provider);
        await _checkingService.LoadSubjectCourseCheckingAsync(subjectCourseId, cancellationToken);

        return list;
    }
}