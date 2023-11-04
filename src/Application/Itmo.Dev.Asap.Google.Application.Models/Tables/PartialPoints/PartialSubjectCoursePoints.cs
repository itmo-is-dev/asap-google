namespace Itmo.Dev.Asap.Google.Application.Models.Tables.PartialPoints;

public record PartialSubjectCoursePoints(
    int AssignmentCount,
    IEnumerable<PartialStudentAssignmentPoints> StudentPoints);