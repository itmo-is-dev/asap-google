using Itmo.Dev.Asap.Google.Application.Abstractions.DataAccess.Repositories;
using Itmo.Dev.Asap.Google.Application.Abstractions.Tables;
using Itmo.Dev.Asap.Google.Application.Models.SubjectCourses;
using Itmo.Dev.Asap.Google.Application.Models.Tables.Queues;
using Itmo.Dev.Asap.Google.Application.SubjectCourses;
using MediatR;
using Microsoft.Extensions.Logging;
using static Itmo.Dev.Asap.Google.Application.Contracts.Queues.Notifications.QueueUpdated;

namespace Itmo.Dev.Asap.Google.Application.Queues;

public class QueueUpdatedHandler : INotificationHandler<Notification>
{
    private readonly ILogger<QueueUpdatedHandler> _logger;
    private readonly ISubjectCourseRepository _subjectCourseRepository;
    private readonly ITableWriter<SubmissionQueue> _tableWriter;

    public QueueUpdatedHandler(
        ILogger<QueueUpdatedHandler> logger,
        ISubjectCourseRepository subjectCourseRepository,
        ITableWriter<SubmissionQueue> tableWriter)
    {
        _logger = logger;
        _subjectCourseRepository = subjectCourseRepository;
        _tableWriter = tableWriter;
    }

    public async Task Handle(Notification notification, CancellationToken cancellationToken)
    {
        try
        {
            await ExecuteAsync(notification, cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError(
                e,
                "Error while updating queue for subject course {SubjectCourseId} group {GroupId}",
                notification.SubjectCourseId,
                notification.GroupId);
        }
    }

    private async Task ExecuteAsync(Notification notification, CancellationToken cancellationToken)
    {
        GoogleSubjectCourse? subjectCourse = await _subjectCourseRepository
            .FindByIdAsync(notification.SubjectCourseId, cancellationToken);

        if (subjectCourse is null)
        {
            _logger.LogWarning(
                "Tried to update google queue for subject course = {SubjectCourseId}, but no spreadsheet found",
                notification.SubjectCourseId);

            return;
        }

        await _tableWriter.UpdateAsync(subjectCourse.SpreadsheetId, notification.SubmissionsSubmissionQueue, cancellationToken);
    }
}