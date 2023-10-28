namespace Itmo.Dev.Asap.Google.Application.Models.Tables.Points;

public record StudentAssignmentPoints(Guid StudentId, IReadOnlyDictionary<Guid, AssignmentPoints> Points);