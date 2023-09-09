using Itmo.Dev.Asap.Frontend.Application.Abstractions.SubjectCourses;
using Itmo.Dev.Asap.Frontend.Application.Events;
using Itmo.Dev.Asap.Frontend.Application.Factories;

namespace Itmo.Dev.Asap.Frontend.Application.SubjectCourses;

internal class SubjectCourseFactory : ISubjectCourseFactory, IDisposable
{
    private readonly EntityFactory<SubjectCourse, Guid> _factory;

    public SubjectCourseFactory(IEventProvider provider)
    {
        _factory = new EntityFactory<SubjectCourse, Guid>(provider);
    }

    public ISubjectCourse Create(Guid id)
    {
        return _factory.Create(id);
    }

    public void Dispose()
    {
        _factory.Dispose();
    }
}