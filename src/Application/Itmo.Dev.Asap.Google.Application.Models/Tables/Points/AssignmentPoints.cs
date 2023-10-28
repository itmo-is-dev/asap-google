namespace Itmo.Dev.Asap.Google.Application.Models.Tables.Points;

public record AssignmentPoints(Guid AssignmentId, DateOnly Date, bool IsBanned, double Points);