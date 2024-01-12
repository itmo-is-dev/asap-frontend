using Itmo.Dev.Asap.Frontend.Application.Abstractions.Checking.Models;
using Itmo.Dev.Asap.Frontend.Application.Events;

namespace Itmo.Dev.Asap.Frontend.Application.Abstractions.Checking.Events;

public record SubjectCourseCheckingsLoaded(
    Guid SubjectCourseId,
    IEnumerable<SubjectCourseChecking> Checkings) : IApplicationEvent;