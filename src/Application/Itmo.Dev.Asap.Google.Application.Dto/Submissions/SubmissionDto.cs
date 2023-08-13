namespace Itmo.Dev.Asap.Google.Application.Dto.Submissions;

public record SubmissionDto(
    Guid Id,
    DateTime SubmissionDate,
    string Payload,
    string AssignmentShortName,
    SubmissionStateDto State);