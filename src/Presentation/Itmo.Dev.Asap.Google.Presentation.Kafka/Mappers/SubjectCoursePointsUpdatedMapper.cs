using Itmo.Dev.Asap.Google.Application.Dto.SubjectCourses;
using Itmo.Dev.Asap.Google.Application.Dto.Users;
using Itmo.Dev.Asap.Google.Application.Github.Models;
using Itmo.Dev.Asap.Kafka;
using Riok.Mapperly.Abstractions;
using static Itmo.Dev.Asap.Google.Application.Contracts.SubjectCourses.Notifications.SubjectCoursePointsUpdated;

namespace Itmo.Dev.Asap.Google.Presentation.Kafka.Mappers;

[Mapper]
internal static partial class SubjectCoursePointsUpdatedMapper
{
    public static Notification MapTo(
        this SubjectCoursePointsUpdatedValue message,
        IReadOnlyDictionary<Guid, GithubUserDto> githubUsers)
    {
        var assignments = message.Points.Assignments
            .Select(x => x.ToDto())
            .ToDictionary(x => x.Id);

        var students = message.Points.Students
            .Select(x =>
            {
                UserDto user = x.User.ToDto();

                string? githubUsername = githubUsers.TryGetValue(user.Id, out GithubUserDto? githubUser)
                    ? githubUser.GithubUsername
                    : null;

                return new SubjectCoursePointsDto.StudentDto(user, x.GroupName, githubUsername);
            })
            .ToDictionary(x => x.User.Id);

        SubjectCoursePointsDto.StudentPointsDto[] studentPoints = message.Points.Points
            .Select(x => x.ToDto())
            .ToArray();

        var points = new SubjectCoursePointsDto(assignments, students, studentPoints);

        return new Notification(Guid.Parse(message.SubjectCourseId), points);
    }

    private static partial AssignmentDto ToDto(this SubjectCoursePointsUpdatedValue.Types.Assignment assignment);

    private static partial UserDto ToDto(this SubjectCoursePointsUpdatedValue.Types.User user);

    private static partial SubjectCoursePointsDto.StudentPointsDto ToDto(
        this SubjectCoursePointsUpdatedValue.Types.StudentPoints points);
}