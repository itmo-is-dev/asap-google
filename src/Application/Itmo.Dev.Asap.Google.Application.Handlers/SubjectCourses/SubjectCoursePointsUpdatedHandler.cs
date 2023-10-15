using Itmo.Dev.Asap.Google.Application.Abstractions;
using Itmo.Dev.Asap.Google.Application.DataAccess;
using Itmo.Dev.Asap.Google.Application.Dto.SubjectCourses;
using Itmo.Dev.Asap.Google.Domain.Assignments;
using Itmo.Dev.Asap.Google.Domain.Students;
using Itmo.Dev.Asap.Google.Domain.SubjectCourses;
using MediatR;
using Microsoft.Extensions.Logging;
using static Itmo.Dev.Asap.Google.Application.Contracts.SubjectCourses.Notifications.SubjectCoursePointsUpdated;

namespace Itmo.Dev.Asap.Google.Application.Handlers.SubjectCourses;

internal class SubjectCoursePointsUpdatedHandler : INotificationHandler<Notification>
{
    private readonly ILogger<SubjectCoursePointsUpdatedHandler> _logger;
    private readonly ISheet<SubjectCoursePointsDto> _sheet;
    private readonly IPersistenceContext _context;

    public SubjectCoursePointsUpdatedHandler(
        ILogger<SubjectCoursePointsUpdatedHandler> logger,
        ISheet<SubjectCoursePointsDto> sheet,
        IPersistenceContext context)
    {
        _logger = logger;
        _sheet = sheet;
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
        await _sheet.UpdateAsync(subjectCourse.SpreadsheetId, notification.Points, cancellationToken);

        _logger.LogInformation(
            "Successfully updated points sheet of course {SubjectCourseId}",
            notification.SubjectCourseId);
    }

    private async Task UpdateSubjectCourseEntitiesAsync(Notification notification, CancellationToken cancellationToken)
    {
        SubjectCourseStudent[] students = notification.Points.Students
            .Select((student, i) => new SubjectCourseStudent(
                StudentId: student.Value.User.Id,
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