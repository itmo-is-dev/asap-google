using Itmo.Dev.Asap.Google.Application.Abstractions.DataAccess;
using Itmo.Dev.Asap.Google.Application.Abstractions.Tables;
using Itmo.Dev.Asap.Google.Application.Models.SubjectCourses;
using Itmo.Dev.Asap.Google.Application.Models.Tables.Points;
using Itmo.Dev.Asap.Google.Application.SubjectCourses;
using MediatR;
using Microsoft.Extensions.Logging;
using static Itmo.Dev.Asap.Google.Application.Contracts.SubjectCourses.Notifications.SubjectCoursePointsUpdated;

namespace Itmo.Dev.Asap.Google.Application.Points;

internal class PointsUpdatedHandler : INotificationHandler<Notification>
{
    private readonly ILogger<PointsUpdatedHandler> _logger;
    private readonly ITableWriter<SubjectCoursePoints> _tableWriter;
    private readonly IPersistenceContext _context;

    public PointsUpdatedHandler(
        ILogger<PointsUpdatedHandler> logger,
        ITableWriter<SubjectCoursePoints> tableWriter,
        IPersistenceContext context)
    {
        _logger = logger;
        _tableWriter = tableWriter;
        _context = context;
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

        await UpdateSubjectCourseEntitiesAsync(notification, cancellationToken);
        await _tableWriter.UpdateAsync(subjectCourse.SpreadsheetId, notification.Points, cancellationToken);

        _logger.LogInformation(
            "Successfully updated points sheet of course {SubjectCourseId}",
            notification.SubjectCourseId);
    }

    private async Task UpdateSubjectCourseEntitiesAsync(Notification notification, CancellationToken cancellationToken)
    {
        SubjectCourseStudent[] students = notification.Points.StudentPoints
            .Select((student, i) => new SubjectCourseStudent(
                StudentId: student.Value.StudentId,
                SubjectCourseId: notification.SubjectCourseId,
                Ordinal: i))
            .ToArray();

        SubjectCourseAssignment[] assignments = notification.Points.Assignments
            .Select((assignment, i) => new SubjectCourseAssignment(
                SubjectCourseId: notification.SubjectCourseId,
                AssignmentId: assignment.Value.Id,
                Ordinal: i))
            .ToArray();

        _context.SubjectCourseStudents.AddOrUpdateRange(students);
        _context.SubjectCourseAssignments.AddOrUpdateRange(assignments);

        await _context.CommitAsync(cancellationToken);
    }
}