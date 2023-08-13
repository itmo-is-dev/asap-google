using Itmo.Dev.Asap.Google.Application.Dto.SubjectCourses;
using Itmo.Dev.Asap.Google.Domain.SubjectCourses;

namespace Itmo.Dev.Asap.Google.Application.Mapping;

public static class SubjectCourseMapping
{
    public static GoogleSubjectCourseDto ToDto(this GoogleSubjectCourse course)
    {
        return new GoogleSubjectCourseDto(course.Id, course.SpreadsheetId);
    }
}