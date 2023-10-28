using Itmo.Dev.Asap.Google.Application.Abstractions.Enrichment;
using Itmo.Dev.Asap.Google.Application.Abstractions.Spreadsheets.Models;
using Itmo.Dev.Asap.Google.Application.Abstractions.Spreadsheets.Services;
using Itmo.Dev.Asap.Google.Application.Models.SubjectCourses;

namespace Itmo.Dev.Asap.Google.Application.Enrichment;

public class GoogleSubjectCourseEnricher : IGoogleSubjectCourseEnricher
{
    private readonly ISpreadsheetService _service;

    public GoogleSubjectCourseEnricher(ISpreadsheetService service)
    {
        _service = service;
    }

    public async Task<GoogleSubjectCourseDto> EnrichAsync(
        GoogleSubjectCourse subjectCourse,
        CancellationToken cancellationToken)
    {
        GoogleSpreadsheet? spreadsheet = await _service
            .FindSpreadsheetAsync(subjectCourse.SpreadsheetId, cancellationToken);

        return new GoogleSubjectCourseDto(
            subjectCourse.Id,
            subjectCourse.SpreadsheetId,
            spreadsheet?.Name ?? string.Empty);
    }
}