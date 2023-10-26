using Itmo.Dev.Asap.Google.Application.Abstractions;
using Itmo.Dev.Asap.Google.Application.DataAccess.Repositories;
using Itmo.Dev.Asap.Google.Application.Dto.Submissions;
using Itmo.Dev.Asap.Google.Domain.SubjectCourses;
using MediatR;
using Microsoft.Extensions.Logging;
using static Itmo.Dev.Asap.Google.Application.Contracts.Queues.Notifications.QueueUpdated;

namespace Itmo.Dev.Asap.Google.Application.Handlers.Queues;

public class QueueUpdatedHandler : INotificationHandler<Notification>
{
    private readonly ILogger<QueueUpdatedHandler> _logger;
    private readonly ISubjectCourseRepository _subjectCourseRepository;
    private readonly ITableWriter<SubmissionsQueueDto> _tableWriter;

    public QueueUpdatedHandler(
        ILogger<QueueUpdatedHandler> logger,
        ISubjectCourseRepository subjectCourseRepository,
        ITableWriter<SubmissionsQueueDto> tableWriter)
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

        await _tableWriter.UpdateAsync(subjectCourse.SpreadsheetId, notification.SubmissionsQueue, cancellationToken);
    }
}