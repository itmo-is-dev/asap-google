namespace Itmo.Dev.Asap.Google.Application.Models.Tables.Points;

public record PointsStudent(
    Guid Id,
    string FirstName,
    string MiddleName,
    string LastName,
    int? UniversityId,
    string GroupName,
    string? GithubUserName)
{
    public string FullName => $"{LastName} {FirstName} {MiddleName}";
}