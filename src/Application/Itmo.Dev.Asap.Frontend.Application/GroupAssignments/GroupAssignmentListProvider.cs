using Itmo.Dev.Asap.Frontend.Application.Abstractions.GroupAssignments;
using Itmo.Dev.Asap.Frontend.Application.Events;

namespace Itmo.Dev.Asap.Frontend.Application.GroupAssignments;

internal class GroupAssignmentListProvider : IGroupAssignmentListProvider
{
    private readonly IEventProvider _provider;
    private readonly IGroupAssignmentService _service;

    public GroupAssignmentListProvider(IEventProvider provider, IGroupAssignmentService service)
    {
        _provider = provider;
        _service = service;
    }

    public async Task<IGroupAssignmentList> LoadAsync(Guid assignmentId, CancellationToken cancellationToken)
    {
        var list = new GroupAssignmentList(assignmentId, _provider);
        await _service.LoadForAssignmentAsync(assignmentId, cancellationToken);

        return list;
    }
}