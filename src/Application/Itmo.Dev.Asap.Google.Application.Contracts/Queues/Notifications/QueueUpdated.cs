using Itmo.Dev.Asap.Google.Application.Models.Tables.Queues;
using MediatR;

namespace Itmo.Dev.Asap.Google.Application.Contracts.Queues.Notifications;

public static class QueueUpdated
{
    public record Notification(Guid SubjectCourseId, Guid GroupId, SubmissionQueue SubmissionsSubmissionQueue) : INotification;
}