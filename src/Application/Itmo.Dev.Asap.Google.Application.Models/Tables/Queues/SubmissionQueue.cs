namespace Itmo.Dev.Asap.Google.Application.Models.Tables.Queues;

public record SubmissionQueue(
    string Title,
    IReadOnlyDictionary<Guid, QueueStudent> Students,
    IReadOnlyList<QueueSubmission> Submissions);