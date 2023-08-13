using Itmo.Dev.Asap.Google.Application.DataAccess;
using Itmo.Dev.Asap.Google.Application.Dto.SubjectCourses;
using Itmo.Dev.Asap.Google.Application.Spreadsheets.Models;
using Itmo.Dev.Asap.Google.Application.Spreadsheets.Services;
using Itmo.Dev.Asap.Google.Domain.SubjectCourses;
using MediatR;
using static Itmo.Dev.Asap.Google.Application.Contracts.SubjectCourses.Notifications.SubjectCoursesCreated;

namespace Itmo.Dev.Asap.Google.Application.Handlers.SubjectCourses;

internal class SubjectCourseCreatedHandler : INotificationHandler<Notification>
{
    private readonly ISpreadsheetService _spreadsheetService;
    private readonly IPersistenceContext _context;

    public SubjectCourseCreatedHandler(
        ISpreadsheetService spreadsheetService,
        IPersistenceContext context)
    {
        _spreadsheetService = spreadsheetService;
        _context = context;
    }

    public async Task Handle(Notification notification, CancellationToken cancellationToken)
    {
        foreach (SubjectCourseDto dto in notification.SubjectCourse)
        {
            SpreadsheetCreateResult result = await _spreadsheetService
                .CreateSpreadsheetAsync(dto.Title, cancellationToken);

            var subjectCourse = new GoogleSubjectCourse(dto.Id, result.SpreadsheetId);
            _context.SubjectCourses.Add(subjectCourse, cancellationToken);
        }

        await _context.CommitAsync(cancellationToken);
    }
}