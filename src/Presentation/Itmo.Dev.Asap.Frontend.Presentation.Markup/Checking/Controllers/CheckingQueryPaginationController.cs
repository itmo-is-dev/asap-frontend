using Itmo.Dev.Asap.Frontend.Application.Abstractions.Checking;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.Checking.Models;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.Models;
using Phazor.Components;

namespace Itmo.Dev.Asap.Frontend.Presentation.Markup.Checking.Controllers;

public class CheckingQueryPaginationController : IForwardPaginatorController<SubjectCourseCheckingResult, PageToken>
{
    private readonly long _checkingId;
    private readonly IReadOnlyCollection<Guid> _assignmentIds;
    private readonly IReadOnlyCollection<Guid> _groupIds;
    private readonly ICheckingResultService _service;

    public CheckingQueryPaginationController(
        ICheckingResultService service,
        long checkingId,
        IReadOnlyCollection<Guid> assignmentIds,
        IReadOnlyCollection<Guid> groupIds)
    {
        _service = service;
        _checkingId = checkingId;
        _assignmentIds = assignmentIds;
        _groupIds = groupIds;
    }

    public PageToken CreateState()
    {
        return new PageToken(null);
    }

    public bool ShouldLoadNextPage(PageToken state)
    {
        return state.Value is not null;
    }

    public async Task<ForwardPageLoadResult<SubjectCourseCheckingResult, PageToken>> LoadPageAsync(
        PageToken state,
        CancellationToken cancellationToken)
    {
        CheckingResultResponse response = await _service.GetCheckingResultsAsync(
            _checkingId,
            _assignmentIds,
            _groupIds,
            state,
            cancellationToken);

        return new ForwardPageLoadResult<SubjectCourseCheckingResult, PageToken>(
            response.Results,
            response.PageToken);
    }
}