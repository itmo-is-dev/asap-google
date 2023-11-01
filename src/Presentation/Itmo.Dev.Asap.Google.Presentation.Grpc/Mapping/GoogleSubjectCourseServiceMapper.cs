using Itmo.Dev.Asap.Google.Application.Contracts.SubjectCourses;
using Itmo.Dev.Asap.Google.Application.Models.SubjectCourses;
using Itmo.Dev.Asap.Google.Common;

namespace Itmo.Dev.Asap.Google.Presentation.Grpc.Mapping;

internal static class GoogleSubjectCourseServiceMapper
{
    public static FindSubjectCoursesById.Query MapToApplicationRequest(this FindByIdsRequest request)
    {
        return new FindSubjectCoursesById.Query(request.Ids.Select(x => x.ToGuid()));
    }

    public static FindByIdsResponse MapToGrpcResponse(this FindSubjectCoursesById.Response response)
    {
        return new FindByIdsResponse
        {
            SubjectCourses = { response.SubjectCourses.Select(Map) },
        };
    }

    private static GoogleSubjectCourse Map(GoogleSubjectCourseDto subjectCourse)
    {
        return new GoogleSubjectCourse
        {
            Id = subjectCourse.Id.ToString(),
            SpreadsheetId = subjectCourse.SpreadsheetId,
            SpreadsheetName = subjectCourse.SpreadsheetName,
        };
    }
}