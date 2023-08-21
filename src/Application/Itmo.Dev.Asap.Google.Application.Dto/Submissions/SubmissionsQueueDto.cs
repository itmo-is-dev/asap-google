using Itmo.Dev.Asap.Google.Application.Dto.Students;

namespace Itmo.Dev.Asap.Google.Application.Dto.Submissions;

public record struct SubmissionsQueueDto(
    string GroupName,
    IReadOnlyDictionary<Guid, StudentDto> Students,
    IReadOnlyList<SubmissionDto> Submissions);