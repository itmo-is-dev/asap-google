namespace Itmo.Dev.Asap.Google.Application.Abstractions.Models;

public record struct CourseStudentsDto(IReadOnlyList<CourseStudentsDto.StudentDto> Students)
{
    public sealed record StudentDto(string GroupName);
}