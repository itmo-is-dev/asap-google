namespace Itmo.Dev.Asap.Google.Application.Models.Tables.Points;

public record SubjectCoursePoints(
    IReadOnlyList<PointsStudent> Students,
    IReadOnlyDictionary<Guid, Assignment> Assignments,
    IReadOnlyDictionary<Guid, StudentAssignmentPoints> StudentPoints);