using Itmo.Dev.Asap.Google.Application.Models.Submissions;

namespace Itmo.Dev.Asap.Google.Application.Models.Tables.Queues;

public record QueueSubmission(
    Guid Id,
    Guid StudentId,
    DateTime SubmissionDate,
    string Payload,
    string AssignmentShortName,
    SubmissionState State,
    int Code);