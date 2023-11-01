using MediatR;

namespace Itmo.Dev.Asap.Google.Application.Contracts.SubjectCourses.Notifications;

internal static class SubjectCoursePointsPartiallyUpdated
{
    public sealed record StudentPoints(
        Guid StudentId,
        Guid AssignmentId,
        DateOnly Date,
        bool IsBanned,
        double Points);

    public record Notification(Guid SubjectCourseId, IReadOnlyCollection<StudentPoints> Points) : INotification;
}