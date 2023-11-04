using Itmo.Dev.Asap.Google.Application.Contracts.SubjectCourses.Notifications;
using Itmo.Dev.Asap.Google.Common;
using Itmo.Dev.Asap.Kafka;
using Itmo.Dev.Platform.Kafka.Consumer;
using Itmo.Dev.Platform.Kafka.Consumer.Models;
using MediatR;

namespace Itmo.Dev.Asap.Google.Presentation.Kafka.Handlers;

public class StudentPointsUpdatedHandler
    : IKafkaMessageHandler<StudentPointsUpdatedKey, StudentPointsUpdatedValue>
{
    private readonly IMediator _mediator;

    public StudentPointsUpdatedHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async ValueTask HandleAsync(
        IEnumerable<ConsumerKafkaMessage<StudentPointsUpdatedKey, StudentPointsUpdatedValue>> messages,
        CancellationToken cancellationToken)
    {
        IEnumerable<SubjectCoursePointsPartiallyUpdated.Notification> notifications = messages
            .GroupBy(message => message.Key.SubjectCourseId, Map);

        foreach (SubjectCoursePointsPartiallyUpdated.Notification notification in notifications)
        {
            await _mediator.Publish(notification, cancellationToken);
        }
    }

    private static SubjectCoursePointsPartiallyUpdated.Notification Map(
        string subjectCourseId,
        IEnumerable<ConsumerKafkaMessage<StudentPointsUpdatedKey, StudentPointsUpdatedValue>> messages)
    {
        IEnumerable<StudentPointsUpdatedValue> values = messages.Select(x => x.Value);
        SubjectCoursePointsPartiallyUpdated.StudentPoints[] points = Map(GetLatest(values)).ToArray();

        return new SubjectCoursePointsPartiallyUpdated.Notification(subjectCourseId.ToGuid(), points);
    }

    private static IEnumerable<StudentPointsUpdatedValue> GetLatest(
        IEnumerable<StudentPointsUpdatedValue> values)
    {
        return values.GroupBy(
            x => (x.StudentId, x.AssignmentId),
            (_, g) => g.OrderByDescending(x => x.Date).First());
    }

    private static IEnumerable<SubjectCoursePointsPartiallyUpdated.StudentPoints> Map(
        IEnumerable<StudentPointsUpdatedValue> values)
    {
        foreach (StudentPointsUpdatedValue value in values)
        {
            if (value.Points is null)
                continue;

            yield return new SubjectCoursePointsPartiallyUpdated.StudentPoints(
                value.StudentId.ToGuid(),
                value.AssignmentId.ToGuid(),
                DateOnly.FromDateTime(value.Date.ToDateTime()),
                value.IsBanned,
                value.Points.Value);
        }
    }
}