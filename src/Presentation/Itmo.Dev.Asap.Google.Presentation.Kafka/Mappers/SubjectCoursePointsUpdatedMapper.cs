using Google.Protobuf.WellKnownTypes;
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

        SubjectCoursePointsDto.StudentDto[] students = message.Points.Students
            .Select(x =>
            {
                var userId = Guid.Parse(x.User.Id);

                string? githubUsername = githubUsers.TryGetValue(userId, out GithubUserDto? githubUser)
                    ? githubUser.GithubUsername
                    : null;

                return new SubjectCoursePointsDto.StudentDto(
                    x.User.ToDto(),
                    x.GroupName,
                    githubUsername);
            })
            .ToArray();

        var studentPoints = message.Points.Points
            .Select(p =>
            {
                var points = p.Points
                    .Select(Map)
                    .ToDictionary(x => x.AssignmentId);

                return new SubjectCoursePointsDto.StudentPointsDto(Guid.Parse(p.StudentId), points);
            })
            .ToDictionary(x => x.StudentId);

        return new Notification(
            Guid.Parse(message.SubjectCourseId),
            new SubjectCoursePointsDto(students, assignments, studentPoints));
    }

    private static DateOnly MapToDateOnly(Timestamp timestamp)
        => DateOnly.FromDateTime(timestamp.ToDateTime());

    private static partial AssignmentDto ToDto(this SubjectCoursePointsUpdatedValue.Types.Assignment assignment);

    private static partial UserDto ToDto(this SubjectCoursePointsUpdatedValue.Types.User user);

    private static partial SubjectCoursePointsDto.AssignmentPointsDto Map(
        SubjectCoursePointsUpdatedValue.Types.AssignmentPoints points);
}