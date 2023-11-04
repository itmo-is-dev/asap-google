using Itmo.Dev.Asap.Google.Application.Abstractions.DataAccess;
using Itmo.Dev.Asap.Google.Application.Abstractions.Spreadsheets.Models;
using Itmo.Dev.Asap.Google.Application.Abstractions.Spreadsheets.Services;
using Itmo.Dev.Asap.Google.Application.Models.SubjectCourses;
using MediatR;
using static Itmo.Dev.Asap.Google.Application.Contracts.SubjectCourses.Notifications.SubjectCoursesCreated;

namespace Itmo.Dev.Asap.Google.Application.SubjectCourses;

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
        foreach (SubjectCourse dto in notification.SubjectCourse)
        {
            SpreadsheetCreateResult result = await _spreadsheetService
                .CreateSpreadsheetAsync(dto.Title, cancellationToken);

            var subjectCourse = new GoogleSubjectCourse(dto.Id, result.SpreadsheetId, AssignmentCount: 0);
            _context.SubjectCourses.Add(subjectCourse, cancellationToken);
        }

        await _context.CommitAsync(cancellationToken);
    }
}