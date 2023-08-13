namespace Itmo.Dev.Asap.Google.Application.Dto.Submissions;

public record struct SubmissionsQueueDto(string GroupName, IReadOnlyList<QueueSubmissionDto> Submissions);