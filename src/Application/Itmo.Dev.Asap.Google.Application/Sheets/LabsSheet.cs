using FluentSpreadsheets;
using FluentSpreadsheets.GoogleSheets.Models;
using FluentSpreadsheets.GoogleSheets.Rendering;
using FluentSpreadsheets.Tables;
using Itmo.Dev.Asap.Google.Application.Abstractions;
using Itmo.Dev.Asap.Google.Application.Abstractions.Models;
using Itmo.Dev.Asap.Google.Application.Dto.SubjectCourses;
using Itmo.Dev.Asap.Google.Application.Spreadsheets.Models;
using Itmo.Dev.Asap.Google.Application.Spreadsheets.Services;
using Itmo.Dev.Asap.Google.Common;

namespace Itmo.Dev.Asap.Google.Application.Sheets;

public class LabsSheet : ISheet<SubjectCoursePointsDto>
{
    public const string Title = SheetConfigurations.Labs.Title;

    private readonly ISheet<CourseStudentsDto> _pointsSheet;
    private readonly ITable<SubjectCoursePointsDto> _pointsTable;
    private readonly IGoogleSheetsComponentRenderer _renderer;
    private readonly ISheetService _sheetService;

    public LabsSheet(
        ISheetService sheetService,
        ITable<SubjectCoursePointsDto> pointsTable,
        IGoogleSheetsComponentRenderer renderer,
        ISheet<CourseStudentsDto> pointsSheet)
    {
        _sheetService = sheetService;
        _pointsTable = pointsTable;
        _renderer = renderer;
        _pointsSheet = pointsSheet;
    }

    public async Task UpdateAsync(
        string spreadsheetId,
        SubjectCoursePointsDto model,
        CancellationToken cancellationToken)
    {
        SheetId sheetId = await _sheetService.CreateOrClearSheetAsync(spreadsheetId, Title, cancellationToken);

        IComponent component = _pointsTable.Render(model);
        var sheetInfo = new SheetInfo(spreadsheetId, sheetId.Value, Title);

        await _renderer.RenderAsync(component, sheetInfo, cancellationToken);

        bool pointsSheetExists = await _sheetService
            .SheetExistsAsync(spreadsheetId, PointsSheet.Title, cancellationToken);

        if (pointsSheetExists is false)
        {
            CourseStudentsDto.StudentDto[] students = model.Students.Values
                .Select(x => new CourseStudentsDto.StudentDto(x.GroupName))
                .ToArray();

            var courseStudents = new CourseStudentsDto(students);
            await _pointsSheet.UpdateAsync(spreadsheetId, courseStudents, cancellationToken);
        }
    }
}