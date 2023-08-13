using Itmo.Dev.Asap.Google.Application.Dto.SubjectCourses;
using Riok.Mapperly.Abstractions;
using ProtoMessage = Itmo.Dev.Asap.Kafka.SubjectCourseCreatedValue;

namespace Itmo.Dev.Asap.Google.Presentation.Kafka.Mappers;

[Mapper]
internal static partial class SubjectCourseCreatedMapper
{
    public static partial SubjectCourseDto MapTo(this ProtoMessage.Types.SubjectCourse message);
}