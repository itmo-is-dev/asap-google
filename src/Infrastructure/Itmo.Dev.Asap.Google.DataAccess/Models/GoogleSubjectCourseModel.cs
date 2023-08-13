using Itmo.Dev.Asap.Google.Domain.SubjectCourses;

namespace Itmo.Dev.Asap.Google.DataAccess.Models;

public record GoogleSubjectCourseModel(Guid Id, string SpreadsheetId)
{
    public GoogleSubjectCourse ToEntity()
    {
        return new GoogleSubjectCourse(Id, SpreadsheetId);
    }
}