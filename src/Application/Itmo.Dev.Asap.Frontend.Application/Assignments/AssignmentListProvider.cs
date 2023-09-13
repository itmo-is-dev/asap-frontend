using Itmo.Dev.Asap.Frontend.Application.Abstractions.Assignments;
using Itmo.Dev.Asap.Frontend.Application.Events;

namespace Itmo.Dev.Asap.Frontend.Application.Assignments;

internal class AssignmentListProvider : IAssignmentListProvider
{
    private readonly IEventProvider _provider;
    private readonly IAssignmentService _service;

    public AssignmentListProvider(IEventProvider provider, IAssignmentService service)
    {
        _provider = provider;
        _service = service;
    }

    public async Task<IAssignmentList> LoadAsync(Guid subjectCourseId, CancellationToken cancellationToken)
    {
        var list = new AssignmentList(subjectCourseId, _provider);
        await _service.LoadForSubjectCourseId(subjectCourseId, cancellationToken);

        return list;
    }
}