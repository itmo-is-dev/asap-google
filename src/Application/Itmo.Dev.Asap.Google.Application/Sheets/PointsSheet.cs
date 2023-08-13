using FluentSpreadsheets;
using FluentSpreadsheets.GoogleSheets.Rendering;
using FluentSpreadsheets.Rendering;
using FluentSpreadsheets.Tables;
using Itmo.Dev.Asap.Google.Application.Abstractions;
using Itmo.Dev.Asap.Google.Application.Abstractions.Models;
using Itmo.Dev.Asap.Google.Application.Spreadsheets.Models;
using Itmo.Dev.Asap.Google.Application.Spreadsheets.Services;
using Itmo.Dev.Asap.Google.Common;

namespace Itmo.Dev.Asap.Google.Application.Sheets;

public class PointsSheet : ISheet<CourseStudentsDto>
{
    public const string Title = SheetConfigurations.Points.Title;

    private readonly ITable<CourseStudentsDto> _pointsTable;
    private readonly IComponentRenderer<GoogleSheetRenderCommand> _renderer;

    private readonly ISheetService _sheetService;

    public PointsSheet(
        ISheetService sheetService,
        ITable<CourseStudentsDto> pointsTable,
        IComponentRenderer<GoogleSheetRenderCommand> renderer)
    {
        _sheetService = sheetService;
        _pointsTable = pointsTable;
        _renderer = renderer;
    }

    public async Task UpdateAsync(string spreadsheetId, CourseStudentsDto model, CancellationToken token)
    {
        SheetId sheetId = await _sheetService.CreateSheetAsync(spreadsheetId, Title, token);

        IComponent sheetData = _pointsTable.Render(model);
        var renderCommand = new GoogleSheetRenderCommand(spreadsheetId, sheetId.Value, Title, sheetData);
        await _renderer.RenderAsync(renderCommand, token);
    }
}