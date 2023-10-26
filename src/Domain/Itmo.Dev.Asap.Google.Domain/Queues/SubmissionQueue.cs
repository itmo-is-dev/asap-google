using Itmo.Dev.Asap.Google.Domain.Students;

namespace Itmo.Dev.Asap.Google.Domain.Queues;

public record struct SubmissionQueue(
    IReadOnlyDictionary<Guid, Student> Students,
    IReadOnlyList<Submission> Submissions);