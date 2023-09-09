using Itmo.Dev.Asap.Frontend.Application.Abstractions.SubjectCourseGroups;
using Itmo.Dev.Asap.Frontend.Application.Events;

namespace Itmo.Dev.Asap.Frontend.Application.SubjectCourseGroups;

internal class SubjectCourseGroupListProvider : ISubjectCourseGroupListProvider
{
    private readonly IEventProvider _provider;
    private readonly ISubjectCourseGroupService _service;

    public SubjectCourseGroupListProvider(IEventProvider provider, ISubjectCourseGroupService service)
    {
        _provider = provider;
        _service = service;
    }

    public async ValueTask<ISubjectCourseGroupList> LoadAsync(Guid subjectCourseId, CancellationToken cancellationToken)
    {
        var list = new SubjectCourseGroupList(subjectCourseId, _provider);
        await _service.LoadForSubjectCourseAsync(subjectCourseId, cancellationToken);

        return list;
    }
}