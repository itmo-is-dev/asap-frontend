using Itmo.Dev.Asap.Frontend.Application.Abstractions.Subjects;

namespace Itmo.Dev.Asap.Frontend.Application.Subjects;

internal class SubjectProvider : ISubjectProvider
{
    private readonly ISubjectFactory _factory;
    private readonly ISubjectService _service;

    public SubjectProvider(ISubjectFactory factory, ISubjectService service)
    {
        _factory = factory;
        _service = service;
    }

    public async Task<ISubject> LoadAsync(Guid subjectId, CancellationToken cancellationToken)
    {
        ISubject subject = _factory.Create(subjectId);
        await _service.LoadAsync(subjectId, cancellationToken);

        return subject;
    }
}