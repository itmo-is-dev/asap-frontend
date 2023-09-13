using Itmo.Dev.Asap.Frontend.Application.Abstractions.SubjectCourses;
using Itmo.Dev.Asap.Frontend.Application.Events;

namespace Itmo.Dev.Asap.Frontend.Application.SubjectCourses;

internal class SubjectCourseListProvider : ISubjectCourseListProvider
{
    private readonly ISubjectCourseService _service;
    private readonly IEventProvider _provider;

    public SubjectCourseListProvider(ISubjectCourseService service, IEventProvider provider)
    {
        _service = service;
        _provider = provider;
    }

    public async Task<ISubjectCourseList> LoadAsync(Guid subjectId, CancellationToken cancellationToken)
    {
        var list = new SubjectCourseList(subjectId, _provider);
        await _service.LoadForSubjectAsync(subjectId, cancellationToken);

        return list;
    }
}