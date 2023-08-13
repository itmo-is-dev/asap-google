using Itmo.Dev.Asap.Google.Application.Dto.SubjectCourses;
using MediatR;

namespace Itmo.Dev.Asap.Google.Application.Contracts.SubjectCourses.Notifications;

internal static class SubjectCoursesCreated
{
    public record Notification(IEnumerable<SubjectCourseDto> SubjectCourse) : INotification;
}