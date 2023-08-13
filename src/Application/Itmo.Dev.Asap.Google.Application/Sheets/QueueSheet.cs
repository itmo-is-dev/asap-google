using FluentSpreadsheets;
using FluentSpreadsheets.GoogleSheets.Rendering;
using FluentSpreadsheets.Rendering;
using FluentSpreadsheets.Tables;
using Itmo.Dev.Asap.Google.Application.Abstractions;
using Itmo.Dev.Asap.Google.Application.Dto.Submissions;
using Itmo.Dev.Asap.Google.Application.Spreadsheets.Models;
using Itmo.Dev.Asap.Google.Application.Spreadsheets.Services;

namespace Itmo.Dev.Asap.Google.Application.Sheets;

public class QueueSheet : ISheet<SubmissionsQueueDto>
{
    private readonly ITable<SubmissionsQueueDto> _queueTable;
    private readonly IComponentRenderer<GoogleSheetRenderCommand> _renderer;
    private readonly ISheetService _sheetService;

    public QueueSheet(
        ISheetService sheetService,
        ITable<SubmissionsQueueDto> queueTable,
        IComponentRenderer<GoogleSheetRenderCommand> renderer)
    {
        _sheetService = sheetService;
        _queueTable = queueTable;
        _renderer = renderer;
    }

    public async Task UpdateAsync(string spreadsheetId, SubmissionsQueueDto model, CancellationToken token)
    {
        string title = model.GroupName;

        SheetId sheetId = await _sheetService.CreateOrClearSheetAsync(spreadsheetId, title, token);

        IComponent sheetData = _queueTable.Render(model);
        var renderCommand = new GoogleSheetRenderCommand(spreadsheetId, sheetId.Value, title, sheetData);
        await _renderer.RenderAsync(renderCommand, token);
    }
}