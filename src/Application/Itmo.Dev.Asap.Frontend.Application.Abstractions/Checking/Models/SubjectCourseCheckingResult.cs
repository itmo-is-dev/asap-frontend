namespace Itmo.Dev.Asap.Frontend.Application.Abstractions.Checking.Models;

public record SubjectCourseCheckingResult(
    Guid AssignmentId,
    string AssignmentName,
    CheckingResultSubmissionInfo FirstSubmission,
    CheckingResultSubmissionInfo SecondSubmission,
    double SimilarityScore);