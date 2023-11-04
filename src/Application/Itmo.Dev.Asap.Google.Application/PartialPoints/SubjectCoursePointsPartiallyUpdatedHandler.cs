using Itmo.Dev.Asap.Google.Application.Abstractions.DataAccess;
using Itmo.Dev.Asap.Google.Application.Abstractions.DataAccess.Queries;
using Itmo.Dev.Asap.Google.Application.Abstractions.Tables;
using Itmo.Dev.Asap.Google.Application.Models.SubjectCourses;
using Itmo.Dev.Asap.Google.Application.Models.Tables.PartialPoints;
using Itmo.Dev.Asap.Google.Application.Models.Tables.Points;
using MediatR;
using Microsoft.Extensions.Logging;
using static
    Itmo.Dev.Asap.Google.Application.Contracts.SubjectCourses.Notifications.SubjectCoursePointsPartiallyUpdated;

namespace Itmo.Dev.Asap.Google.Application.PartialPoints;

internal class SubjectCoursePointsPartiallyUpdatedHandler : INotificationHandler<Notification>
{
    private readonly IPersistenceContext _context;
    private readonly ILogger<SubjectCoursePointsPartiallyUpdatedHandler> _logger;
    private readonly ITableWriter<PartialSubjectCoursePoints> _tableWriter;

    public SubjectCoursePointsPartiallyUpdatedHandler(
        IPersistenceContext context,
        ILogger<SubjectCoursePointsPartiallyUpdatedHandler> logger,
        ITableWriter<PartialSubjectCoursePoints> tableWriter)
    {
        _context = context;
        _logger = logger;
        _tableWriter = tableWriter;
    }

    public async Task Handle(Notification notification, CancellationToken cancellationToken)
    {
        GoogleSubjectCourse? subjectCourse = await _context.SubjectCourses
            .FindByIdAsync(notification.SubjectCourseId, cancellationToken);

        if (subjectCourse is null)
        {
            _logger.LogWarning(
                "Tried to update google points for subject course = {SubjectCourseId}, but no spreadsheet found",
                notification.SubjectCourseId);

            return;
        }

        var assignmentQuery = SubjectCourseAssignmentsQuery.Build(builder => builder
            .WithAssignmentIds(notification.Points.Select(x => x.AssignmentId)));

        var studentsQuery = SubjectCourseStudentsQuery.Build(builder => builder
            .WithStudentIds(notification.Points.Select(x => x.StudentId)));

        Dictionary<Guid, int> assignments = await _context.SubjectCourseAssignments
            .QueryAsync(query: assignmentQuery, cancellationToken: cancellationToken)
            .ToDictionaryAsync(
                keySelector: assignment => assignment.AssignmentId,
                elementSelector: assignment => assignment.Ordinal,
                cancellationToken: cancellationToken);

        Dictionary<Guid, int> students = await _context.SubjectCourseStudents
            .QueryAsync(query: studentsQuery, cancellationToken: cancellationToken)
            .ToDictionaryAsync(
                keySelector: student => student.StudentId,
                elementSelector: student => student.Ordinal,
                cancellationToken: cancellationToken);

        IEnumerable<PartialStudentAssignmentPoints> points = notification.Points
            .GroupBy(x => x.StudentId)
            .Select(group => new PartialStudentAssignmentPoints(
                students[group.Key],
                group.Select(points => Map(points, assignments))));

        var subjectCoursePoints = new PartialSubjectCoursePoints(subjectCourse.AssignmentCount, points);

        await _tableWriter.UpdateAsync(subjectCourse.SpreadsheetId, subjectCoursePoints, cancellationToken);
    }

    private static PartialAssignmentPoints Map(StudentPoints points, IReadOnlyDictionary<Guid, int> assignments)
    {
        int assignmentOrdinal = assignments[points.AssignmentId];
        var assignmentPoints = new AssignmentPoints(points.AssignmentId, points.Date, points.IsBanned, points.Points);

        return new PartialAssignmentPoints(assignmentOrdinal, assignmentPoints);
    }
}