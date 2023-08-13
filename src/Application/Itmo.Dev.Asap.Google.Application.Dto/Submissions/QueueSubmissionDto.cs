using Itmo.Dev.Asap.Google.Application.Dto.Students;

namespace Itmo.Dev.Asap.Google.Application.Dto.Submissions;

public record QueueSubmissionDto(StudentDto Student, SubmissionDto Submission);