using Itmo.Dev.Asap.Frontend.Application.Abstractions.Students.Models;

namespace Itmo.Dev.Asap.Frontend.Application.Abstractions.Checking.Models;

public record struct CheckingResultSubmissionInfo(Guid SubmissionId, StudentData Student);