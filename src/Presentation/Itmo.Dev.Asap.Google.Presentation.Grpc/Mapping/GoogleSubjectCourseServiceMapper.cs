using Itmo.Dev.Asap.Google.Application.Contracts.SubjectCourses;
using Riok.Mapperly.Abstractions;

namespace Itmo.Dev.Asap.Google.Presentation.Grpc.Mapping;

[Mapper]
internal static partial class GoogleSubjectCourseServiceMapper
{
    public static partial FindSubjectCoursesById.Query MapToApplicationRequest(this FindByIdsRequest request);

    public static partial FindByIdsResponse MapToGrpcResponse(this FindSubjectCoursesById.Response response);
}