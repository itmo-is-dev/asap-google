using MediatR;

namespace Itmo.Dev.Asap.Google.Application.Contracts.SubjectCourses.Notifications;

internal static class SubjectCoursesCreated
{
    public sealed record SubjectCourse(Guid Id, string Title);

    public record Notification(IEnumerable<SubjectCourse> SubjectCourse) : INotification;
}