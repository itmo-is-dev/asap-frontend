using Itmo.Dev.Asap.Frontend.Application.Abstractions.SubjectCourseGroups;
using Itmo.Dev.Asap.Frontend.Application.Events;
using Itmo.Dev.Asap.Frontend.Application.Factories;

namespace Itmo.Dev.Asap.Frontend.Application.SubjectCourseGroups;

internal class SubjectCourseGroupFactory : ISubjectCourseGroupFactory, IDisposable
{
    private readonly EntityFactory<SubjectCourseGroup, (Guid SubjectCourseId, Guid StudentGroupId)> _factory;

    public SubjectCourseGroupFactory(IEventProvider provider)
    {
        _factory = new EntityFactory<SubjectCourseGroup, (Guid SubjectCourseId, Guid StudentGroupId)>(provider);
    }

    public ISubjectCourseGroup Create(Guid subjectCourseId, Guid studentGroupId)
    {
        return _factory.Create((subjectCourseId, studentGroupId));
    }

    public void Dispose()
    {
        _factory.Dispose();
    }
}