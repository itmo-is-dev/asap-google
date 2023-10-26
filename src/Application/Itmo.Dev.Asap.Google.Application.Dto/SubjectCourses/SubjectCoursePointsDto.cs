using Itmo.Dev.Asap.Google.Application.Dto.Users;

namespace Itmo.Dev.Asap.Google.Application.Dto.SubjectCourses;

public record struct SubjectCoursePointsDto(
    IReadOnlyList<SubjectCoursePointsDto.StudentDto> Students,
    IReadOnlyDictionary<Guid, AssignmentDto> Assignments,
    IReadOnlyDictionary<Guid, SubjectCoursePointsDto.StudentPointsDto> StudentPoints)
{
    public sealed record StudentDto(UserDto User, string GroupName, string? GithubUsername);

    public sealed record StudentPointsDto(Guid StudentId, IReadOnlyDictionary<Guid, AssignmentPointsDto> Points);

    public sealed record AssignmentPointsDto(Guid AssignmentId, DateOnly Date, bool IsBanned, double Points);
}