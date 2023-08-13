using Itmo.Dev.Asap.Google.Application.Dto.Submissions;
using MediatR;

namespace Itmo.Dev.Asap.Google.Application.Contracts.Queues.Notifications;

public static class QueueUpdated
{
    public record Notification(Guid SubjectCourseId, Guid GroupId, SubmissionsQueueDto SubmissionsQueue) : INotification;
}