namespace Itmo.Dev.Asap.Google.Application.Dto.Students;

public record StudentPointsDto(StudentDto Student, IReadOnlyCollection<AssignmentPointsDto> Points);