namespace Itmo.Dev.Asap.Google.Application.Models.Tables.PartialPoints;

public record PartialStudentAssignmentPoints(int StudentOrdinal, IEnumerable<PartialAssignmentPoints> Points);