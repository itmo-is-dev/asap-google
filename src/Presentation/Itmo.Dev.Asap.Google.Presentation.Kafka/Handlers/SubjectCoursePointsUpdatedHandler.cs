using Itmo.Dev.Asap.Google.Application.Abstractions.Github.Models;
using Itmo.Dev.Asap.Google.Application.Abstractions.Github.Services;
using Itmo.Dev.Asap.Google.Application.Models.Tables.Points;
using Itmo.Dev.Asap.Google.Common;
using Itmo.Dev.Asap.Kafka;
using Itmo.Dev.Platform.Kafka.Consumer;
using Itmo.Dev.Platform.Kafka.Extensions;
using MediatR;
using static Itmo.Dev.Asap.Google.Application.Contracts.SubjectCourses.Notifications.SubjectCoursePointsUpdated;

namespace Itmo.Dev.Asap.Google.Presentation.Kafka.Handlers;

public class SubjectCoursePointsUpdatedHandler
    : IKafkaConsumerHandler<SubjectCoursePointsUpdatedKey, SubjectCoursePointsUpdatedValue>
{
    private readonly IMediator _mediator;
    private readonly IGithubUserService _githubUserService;

    public SubjectCoursePointsUpdatedHandler(
        IMediator mediator,
        IGithubUserService githubUserService)
    {
        _mediator = mediator;
        _githubUserService = githubUserService;
    }

    public async ValueTask HandleAsync(
        IEnumerable<IKafkaConsumerMessage<SubjectCoursePointsUpdatedKey, SubjectCoursePointsUpdatedValue>> messages,
        CancellationToken cancellationToken)
    {
        IKafkaConsumerMessage<SubjectCoursePointsUpdatedKey, SubjectCoursePointsUpdatedValue>[] latest = messages
            .GetLatestByKey()
            .ToArray();

        IEnumerable<string> studentIds = latest
            .SelectMany(x => x.Value.Points.Students)
            .Select(x => x.User.Id);

        Dictionary<Guid, GithubUserModel> githubUsers = await _githubUserService
            .FindByIdsAsync(studentIds, cancellationToken)
            .ToDictionaryAsync(x => x.Id, cancellationToken);

        IEnumerable<Notification> notifications = latest
            .Select(x => Map(x.Value, githubUsers));

        foreach (Notification notification in notifications)
        {
            await _mediator.Publish(notification, cancellationToken);
        }
    }

    private static Notification Map(
        SubjectCoursePointsUpdatedValue value,
        IReadOnlyDictionary<Guid, GithubUserModel> githubUsers)
    {
        return new Notification(
            value.SubjectCourseId.ToGuid(),
            Map(value.Points, githubUsers));
    }

    private static SubjectCoursePoints Map(
        SubjectCoursePointsUpdatedValue.Types.SubjectCoursePoints points,
        IReadOnlyDictionary<Guid, GithubUserModel> githubUsers)
    {
        var assignments = points.Assignments
            .Select(Map)
            .ToDictionary(x => x.Id);

        PointsStudent[] students = points.Students
            .Select(student =>
            {
                var userId = Guid.Parse(student.User.Id);

                string? githubUsername = githubUsers.TryGetValue(userId, out GithubUserModel? githubUser)
                    ? githubUser.GithubUsername
                    : null;

                return new PointsStudent(
                    userId,
                    student.User.FirstName,
                    student.User.MiddleName,
                    student.User.LastName,
                    student.User.UniversityId,
                    student.GroupName,
                    githubUsername);
            })
            .ToArray();

        var studentPoints = points.Points
            .Select(p =>
            {
                var assignmentPoints = p.Points
                    .Select(Map)
                    .ToDictionary(x => x.AssignmentId);

                return new StudentAssignmentPoints(p.StudentId.ToGuid(), assignmentPoints);
            })
            .ToDictionary(x => x.StudentId);

        return new SubjectCoursePoints(students, assignments, studentPoints);
    }

    private static Assignment Map(SubjectCoursePointsUpdatedValue.Types.Assignment assignment)
        => new Assignment(assignment.Id.ToGuid(), assignment.ShortName);

    private static AssignmentPoints Map(SubjectCoursePointsUpdatedValue.Types.AssignmentPoints points)
    {
        return new AssignmentPoints(
            points.AssignmentId.ToGuid(),
            DateOnly.FromDateTime(points.Date.ToDateTime()),
            points.IsBanned,
            points.Points);
    }
}