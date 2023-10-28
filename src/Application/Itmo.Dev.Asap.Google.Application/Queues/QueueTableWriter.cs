using FluentSpreadsheets;
using FluentSpreadsheets.GoogleSheets.Models;
using FluentSpreadsheets.GoogleSheets.Rendering;
using Itmo.Dev.Asap.Google.Application.Abstractions.Spreadsheets.Models;
using Itmo.Dev.Asap.Google.Application.Abstractions.Spreadsheets.Services;
using Itmo.Dev.Asap.Google.Application.Abstractions.Tables;
using Itmo.Dev.Asap.Google.Application.Models.Tables.Queues;
using Itmo.Dev.Asap.Google.Common.Tools;

namespace Itmo.Dev.Asap.Google.Application.Queues;

public class QueueTableWriter : ITableWriter<Queue>
{
    private readonly IGoogleSheetsComponentRenderer _renderer;
    private readonly ISheetService _sheetService;
    private readonly ICultureInfoProvider _cultureInfoProvider;

    public QueueTableWriter(
        ISheetService sheetService,
        IGoogleSheetsComponentRenderer renderer,
        ICultureInfoProvider cultureInfoProvider)
    {
        _sheetService = sheetService;
        _renderer = renderer;
        _cultureInfoProvider = cultureInfoProvider;
    }

    public async Task UpdateAsync(string spreadsheetId, Queue model, CancellationToken cancellationToken)
    {
        string title = model.Title;

        SheetId sheetId = await _sheetService.CreateOrClearSheetAsync(spreadsheetId, title, cancellationToken);

        IComponent component = new QueueTable(_cultureInfoProvider).Render(model);

        var sheetInfo = new SheetInfo(spreadsheetId, sheetId.Value, title);
        await _renderer.RenderAsync(component, sheetInfo, cancellationToken);
    }
}