using Itmo.Dev.Asap.Frontend.Application.Abstractions.Checking.Models;
using Itmo.Dev.Asap.Frontend.Application.Abstractions.Students.Models;
using Itmo.Dev.Asap.Gateway.Application.Dto.Checking;
using CheckingResultSubmissionInfo =
    Itmo.Dev.Asap.Frontend.Application.Abstractions.Checking.Models.CheckingResultSubmissionInfo;

namespace Itmo.Dev.Asap.Frontend.Integrations.Gateway.Mapping;

public static class CheckingMapper
{
    public static SubjectCourseChecking MapToModel(this CheckingDto checking)
        => new SubjectCourseChecking(checking.Id, checking.CreatedAt, checking.IsCompleted);

    public static SubjectCourseCheckingResult MapToModel(this CheckingResultDto result)
    {
        return new SubjectCourseCheckingResult(
            result.AssignmentName,
            result.FirstSubmission.MapToModel(),
            result.SecondSubmission.MapToModel(),
            result.SimilarityScore);
    }

    public static CheckingSimilarCodeBlocks MapToModel(this SimilarCodeBlocksDto codeBlocks)
    {
        return new CheckingSimilarCodeBlocks(
            codeBlocks.First.MapToModel(),
            codeBlocks.Second.MapToModel(),
            codeBlocks.SimilarityScore);
    }

    public static CheckingResultSubmissionInfo MapToModel(this CheckingResultSubmissionDto submission)
    {
        return new CheckingResultSubmissionInfo(
            submission.SubmissionId,
            new StudentData(submission.StudentId, submission.FirstName, submission.LastName, submission.GroupName));
    }

    public static CheckingCodeBlock MapToModel(this CodeBlockDto codeBlock)
    {
        return new CheckingCodeBlock(
            codeBlock.FilePath,
            codeBlock.LineFrom,
            codeBlock.LineTo,
            codeBlock.Content);
    }
}