using RichEntity.Annotations;

namespace Itmo.Dev.Asap.Google.Domain.SubjectCourses;

public partial class GoogleSubjectCourse : IEntity<Guid>
{
    public GoogleSubjectCourse(Guid id, string spreadsheetId) : this(id)
    {
        SpreadsheetId = spreadsheetId;
    }

    public string SpreadsheetId { get; protected set; }
}