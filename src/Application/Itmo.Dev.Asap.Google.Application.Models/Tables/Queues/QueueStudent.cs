namespace Itmo.Dev.Asap.Google.Application.Models.Tables.Queues;

public record QueueStudent(
    Guid Id,
    string FirstName,
    string MiddleName,
    string LastName,
    string GroupName)
{
    public string FullName => $"{LastName} {FirstName} {MiddleName}";
}