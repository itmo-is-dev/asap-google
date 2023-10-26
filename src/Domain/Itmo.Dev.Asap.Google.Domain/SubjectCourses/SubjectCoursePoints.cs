namespace Itmo.Dev.Asap.Google.Domain.SubjectCourses;

public record struct SubjectCoursePoints(
    IReadOnlyList<SubjectCoursePoints.Student> Students,
    IReadOnlyDictionary<Guid, SubjectCoursePoints.Assignment> Assignments,
    IReadOnlyDictionary<Guid, SubjectCoursePoints.StudentAssignmentPoints> StudentPoints)
{
    public record struct Assignment(Guid Id, string Name);

    public record struct StudentAssignmentPoints(Guid StudentId, IReadOnlyDictionary<Guid, AssignmentPoints> Points);

    public record struct AssignmentPoints(Guid AssignmentId, DateOnly Date, bool IsBanned, double Points);

    public record struct Student(Students.Student Value, string? GithubUserName);
}