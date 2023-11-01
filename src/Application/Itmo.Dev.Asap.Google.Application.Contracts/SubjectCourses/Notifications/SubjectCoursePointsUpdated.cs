using Itmo.Dev.Asap.Google.Application.Models.Tables.Points;
using MediatR;

namespace Itmo.Dev.Asap.Google.Application.Contracts.SubjectCourses.Notifications;

internal static class SubjectCoursePointsUpdated
{
    public record Notification(Guid SubjectCourseId, SubjectCoursePoints Points) : INotification;
}