namespace Itmo.Dev.Asap.Google.Domain.Students;

public readonly record struct Student(
    Guid Id,
    string FirstName,
    string MiddleName,
    string LastName,
    int? UniversityId,
    string GroupName)
{
    public string FullName => $"{LastName} {FirstName} {MiddleName}";
}