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

        SubjectCoursePointsDto.StudentPointsDto[] studentPoints = message.Points.Students
            .Join(
                message.Points.Points,
                x => x.User.Id,
                x => x.StudentId,
                (s, p) => (student: s, points: p.Points))
            .Select(tuple =>
            {
                var userId = Guid.Parse(tuple.student.User.Id);

                string? githubUsername = githubUsers.TryGetValue(userId, out GithubUserDto? githubUser)
                    ? githubUser.GithubUsername
                    : null;

                var student = new SubjectCoursePointsDto.StudentDto(
                    tuple.student.User.ToDto(),
                    tuple.student.GroupName,
                    githubUsername);

                var points = tuple.points.Select(Map).ToDictionary(x => x.AssignmentId);

                return new SubjectCoursePointsDto.StudentPointsDto(student, points);
            })
            .ToArray();

        return new Notification(
            Guid.Parse(message.SubjectCourseId),
            new SubjectCoursePointsDto(assignments, studentPoints));
    }

    private static DateOnly MapToDateOnly(Timestamp timestamp)
        => DateOnly.FromDateTime(timestamp.ToDateTime());

    private static partial AssignmentDto ToDto(this SubjectCoursePointsUpdatedValue.Types.Assignment assignment);

    private static partial UserDto ToDto(this SubjectCoursePointsUpdatedValue.Types.User user);

    private static partial SubjectCoursePointsDto.AssignmentPointsDto Map(
        SubjectCoursePointsUpdatedValue.Types.AssignmentPoints points);
}