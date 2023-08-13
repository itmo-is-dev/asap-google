using Itmo.Dev.Asap.Kafka;
using Riok.Mapperly.Abstractions;
using static Itmo.Dev.Asap.Google.Application.Contracts.SubjectCourses.Notifications.SubjectCoursePointsUpdated;

namespace Itmo.Dev.Asap.Google.Presentation.Kafka.Mappers;

[Mapper]
internal static partial class SubjectCoursePointsUpdatedMapper
{
    public static partial Notification MapTo(this SubjectCoursePointsUpdatedValue message);
}