using FluentSpreadsheets;
using FluentSpreadsheets.GoogleSheets.Rendering;
using FluentSpreadsheets.Rendering;
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
    private readonly IComponentRenderer<GoogleSheetRenderCommand> _renderer;
    private readonly ISheetService _sheetService;

    public LabsSheet(
        ISheetService sheetService,
        ITable<SubjectCoursePointsDto> pointsTable,
        IComponentRenderer<GoogleSheetRenderCommand> renderer,
        ISheet<CourseStudentsDto> pointsSheet)
    {
        _sheetService = sheetService;
        _pointsTable = pointsTable;
        _renderer = renderer;
        _pointsSheet = pointsSheet;
    }

    public async Task UpdateAsync(string spreadsheetId, SubjectCoursePointsDto model, CancellationToken token)
    {
        SheetId sheetId = await _sheetService.CreateOrClearSheetAsync(spreadsheetId, Title, token);

        IComponent sheetData = _pointsTable.Render(model);
        var renderCommand = new GoogleSheetRenderCommand(spreadsheetId, sheetId.Value, Title, sheetData);
        await _renderer.RenderAsync(renderCommand, token);

        bool pointsSheetExists = await _sheetService.SheetExistsAsync(spreadsheetId, PointsSheet.Title, token);

        if (pointsSheetExists is false)
        {
            CourseStudentsDto.StudentDto[] students = model.Students.Values
                .Select(x => new CourseStudentsDto.StudentDto(x.GroupName))
                .ToArray();

            var courseStudents = new CourseStudentsDto(students);
            await _pointsSheet.UpdateAsync(spreadsheetId, courseStudents, token);
        }
    }
}