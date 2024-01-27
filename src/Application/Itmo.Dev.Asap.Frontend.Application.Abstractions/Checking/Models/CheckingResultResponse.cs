using Itmo.Dev.Asap.Frontend.Application.Abstractions.Models;

namespace Itmo.Dev.Asap.Frontend.Application.Abstractions.Checking.Models;

public record CheckingResultResponse(IEnumerable<SubjectCourseCheckingResult> Results, PageToken PageToken);