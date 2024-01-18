using Itmo.Dev.Asap.Frontend.Application.Abstractions.Students.Models;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.Submissions;

namespace Itmo.Dev.Asap.Frontend.Application.Abstractions.Checking.Models;

public record struct CheckingResultSubmissionInfo(Guid SubmissionId, StudentData Student, SubmissionState State);