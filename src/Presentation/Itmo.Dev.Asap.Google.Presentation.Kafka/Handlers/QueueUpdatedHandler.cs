using Itmo.Dev.Asap.Google.Application.Models.Submissions;
using Itmo.Dev.Asap.Google.Application.Models.Tables.Queues;
using Itmo.Dev.Asap.Google.Common;
using Itmo.Dev.Asap.Kafka;
using Itmo.Dev.Platform.Kafka.Consumer;
using Itmo.Dev.Platform.Kafka.Extensions;
using MediatR;
using static Itmo.Dev.Asap.Google.Application.Contracts.Queues.Notifications.QueueUpdated;

namespace Itmo.Dev.Asap.Google.Presentation.Kafka.Handlers;

public class QueueUpdatedHandler : IKafkaConsumerHandler<QueueUpdatedKey, QueueUpdatedValue>
{
    private readonly IMediator _mediator;

    public QueueUpdatedHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async ValueTask HandleAsync(
        IEnumerable<IKafkaConsumerMessage<QueueUpdatedKey, QueueUpdatedValue>> messages,
        CancellationToken cancellationToken)
    {
        IEnumerable<Notification> notifications = messages
            .GetLatestByKey()
            .Select(x => Map(x.Value));

        foreach (Notification notification in notifications)
        {
            await _mediator.Publish(notification, cancellationToken);
        }
    }

    private static Notification Map(QueueUpdatedValue value)
    {
        IReadOnlyDictionary<Guid, QueueStudent> students = value.SubmissionsQueue.Students
            .Select(x => Map(x.Value))
            .ToDictionary(x => x.Id);

        IReadOnlyList<QueueSubmission> submissions = value.SubmissionsQueue.Submissions
            .Select(Map)
            .ToArray();

        var queue = new SubmissionQueue(value.StudentGroupName, students, submissions);

        return new Notification(value.SubjectCourseId.ToGuid(), value.StudentGroupId.ToGuid(), queue);
    }

    private static QueueStudent Map(QueueUpdatedValue.Types.Student student)
    {
        return new QueueStudent(
            student.User.Id.ToGuid(),
            student.User.FirstName,
            student.User.MiddleName,
            student.User.LastName,
            student.GroupName);
    }

    private static QueueSubmission Map(
        QueueUpdatedValue.Types.Submission submission)
    {
        return new QueueSubmission(
            submission.Id.ToGuid(),
            submission.StudentId.ToGuid(),
            submission.SubmissionDate.ToDateTime(),
            submission.Payload,
            submission.AssignmentShortName,
            Map(submission.State),
            submission.Code);
    }

    private static SubmissionState Map(QueueUpdatedValue.Types.SubmissionState state)
    {
        return state switch
        {
            QueueUpdatedValue.Types.SubmissionState.None => SubmissionState.Active,
            QueueUpdatedValue.Types.SubmissionState.Active => SubmissionState.Active,
            QueueUpdatedValue.Types.SubmissionState.Inactive => SubmissionState.Inactive,
            QueueUpdatedValue.Types.SubmissionState.Deleted => SubmissionState.Deleted,
            QueueUpdatedValue.Types.SubmissionState.Completed => SubmissionState.Completed,
            QueueUpdatedValue.Types.SubmissionState.Reviewed => SubmissionState.Reviewed,
            QueueUpdatedValue.Types.SubmissionState.Banned => SubmissionState.Banned,
            _ => SubmissionState.Active,
        };
    }
}