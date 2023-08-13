using Itmo.Dev.Asap.Google.Application.Dto.Students;

namespace Itmo.Dev.Asap.Google.Application.Abstractions.Models;

public record struct CourseStudentsDto(IReadOnlyList<StudentPointsDto> StudentsPoints);