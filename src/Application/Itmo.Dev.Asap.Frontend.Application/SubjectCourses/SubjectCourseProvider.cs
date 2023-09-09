using Itmo.Dev.Asap.Frontend.Application.Abstractions.SubjectCourses;

namespace Itmo.Dev.Asap.Frontend.Application.SubjectCourses;

internal class SubjectCourseProvider : ISubjectCourseProvider
{
    private readonly ISubjectCourseFactory _subjectCourseFactory;
    private readonly ISubjectCourseService _service;

    public SubjectCourseProvider(ISubjectCourseFactory subjectCourseFactory, ISubjectCourseService service)
    {
        _subjectCourseFactory = subjectCourseFactory;
        _service = service;
    }

    public async Task<ISubjectCourse> LoadAsync(Guid subjectCourseId, CancellationToken cancellationToken)
    {
        ISubjectCourse subjectCourse = _subjectCourseFactory.Create(subjectCourseId);
        await _service.LoadAsync(subjectCourseId, cancellationToken);

        return subjectCourse;
    }
}