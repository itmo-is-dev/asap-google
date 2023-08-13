namespace Itmo.Dev.Asap.Google.Application.Dto.Students;

public record AssignmentPointsDto(Guid AssignmentId, DateOnly Date, bool IsBanned, double Points);