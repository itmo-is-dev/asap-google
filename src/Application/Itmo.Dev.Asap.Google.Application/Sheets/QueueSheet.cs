using FluentSpreadsheets;
using FluentSpreadsheets.GoogleSheets.Models;
using FluentSpreadsheets.GoogleSheets.Rendering;
using FluentSpreadsheets.Tables;
using Itmo.Dev.Asap.Google.Application.Abstractions;
using Itmo.Dev.Asap.Google.Application.Dto.Submissions;
using Itmo.Dev.Asap.Google.Application.Spreadsheets.Models;
using Itmo.Dev.Asap.Google.Application.Spreadsheets.Services;

namespace Itmo.Dev.Asap.Google.Application.Sheets;

public class QueueSheet : ISheet<SubmissionsQueueDto>
{
    private readonly ITable<SubmissionsQueueDto> _queueTable;
    private readonly IGoogleSheetsComponentRenderer _renderer;
    private readonly ISheetService _sheetService;

    public QueueSheet(
        ISheetService sheetService,
        ITable<SubmissionsQueueDto> queueTable,
        IGoogleSheetsComponentRenderer renderer)
    {
        _sheetService = sheetService;
        _queueTable = queueTable;
        _renderer = renderer;
    }

    public async Task UpdateAsync(string spreadsheetId, SubmissionsQueueDto model, CancellationToken cancellationToken)
    {
        string title = model.GroupName;

        SheetId sheetId = await _sheetService.CreateOrClearSheetAsync(spreadsheetId, title, cancellationToken);

        IComponent component = _queueTable.Render(model);
        var sheetInfo = new SheetInfo(spreadsheetId, sheetId.Value, title);

        await _renderer.RenderAsync(component, sheetInfo, cancellationToken);
    }
}