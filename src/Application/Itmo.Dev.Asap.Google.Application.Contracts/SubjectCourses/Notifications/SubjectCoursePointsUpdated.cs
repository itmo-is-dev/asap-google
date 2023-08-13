using Itmo.Dev.Asap.Google.Application.Dto.SubjectCourses;
using MediatR;

namespace Itmo.Dev.Asap.Google.Application.Contracts.SubjectCourses.Notifications;

internal static class SubjectCoursePointsUpdated
{
    public record Notification(Guid SubjectCourseId, SubjectCoursePointsDto Points) : INotification;
}