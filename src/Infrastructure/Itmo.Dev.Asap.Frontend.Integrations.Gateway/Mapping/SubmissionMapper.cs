using Itmo.Dev.Asap.Frontend.Application.Abstractions.Submissions;
using Itmo.Dev.Asap.Gateway.Application.Dto.Study;

namespace Itmo.Dev.Asap.Frontend.Integrations.Gateway.Mapping;

public static class SubmissionMapper
{
    public static SubmissionState MapToModel(this SubmissionStateDto state)
    {
        return state switch
        {
            SubmissionStateDto.Active => SubmissionState.Active,
            SubmissionStateDto.Inactive => SubmissionState.Inactive,
            SubmissionStateDto.Deleted => SubmissionState.Deleted,
            SubmissionStateDto.Completed => SubmissionState.Completed,
            SubmissionStateDto.Reviewed => SubmissionState.Reviewed,
            SubmissionStateDto.Banned => SubmissionState.Banned,
            _ => throw new ArgumentOutOfRangeException(nameof(state), state, null),
        };
    }
}