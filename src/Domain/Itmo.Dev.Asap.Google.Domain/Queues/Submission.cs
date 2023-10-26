namespace Itmo.Dev.Asap.Google.Domain.Queues;

public record struct Submission(
    Guid Id,
    Guid StudentId,
    DateTime SubmissionDate,
    string Payload,
    string AssignmentShortName,
    SubmissionState State,
    int Code);