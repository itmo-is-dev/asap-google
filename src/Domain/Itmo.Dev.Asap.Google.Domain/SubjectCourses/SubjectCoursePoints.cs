namespace Itmo.Dev.Asap.Google.Domain.SubjectCourses;

public record struct SubjectCoursePoints(
    IReadOnlyDictionary<Guid, SubjectCoursePoints.Assignment> Assignments,
    IReadOnlyList<SubjectCoursePoints.StudentPoints> Students)
{
    public record struct Assignment(Guid Id, string Name);

    public record struct StudentPoints(
        Students.Student Student,
        string? GithubUserName,
        IReadOnlyDictionary<Guid, AssignmentPoints> Points);

    public record struct AssignmentPoints(Guid AssignmentId, DateOnly Date, bool IsBanned, double Points);
}