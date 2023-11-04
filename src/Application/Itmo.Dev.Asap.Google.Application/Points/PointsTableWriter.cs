using FluentSpreadsheets;
using FluentSpreadsheets.GoogleSheets.Models;
using FluentSpreadsheets.GoogleSheets.Rendering;
using Itmo.Dev.Asap.Google.Application.Abstractions.Spreadsheets.Models;
using Itmo.Dev.Asap.Google.Application.Abstractions.Spreadsheets.Services;
using Itmo.Dev.Asap.Google.Application.Abstractions.Tables;
using Itmo.Dev.Asap.Google.Application.Models.Tables.Points;
using Itmo.Dev.Asap.Google.Common;
using Itmo.Dev.Asap.Google.Common.Tools;

namespace Itmo.Dev.Asap.Google.Application.Points;

public class PointsTableWriter : ITableWriter<SubjectCoursePoints>
{
    private const string Title = SheetConfigurations.Points.Title;

    private readonly IGoogleSheetsComponentRenderer _renderer;
    private readonly ISheetService _sheetService;
    private readonly ICultureInfoProvider _cultureInfoProvider;

    public PointsTableWriter(
        ISheetService sheetService,
        IGoogleSheetsComponentRenderer renderer,
        ICultureInfoProvider cultureInfoProvider)
    {
        _sheetService = sheetService;
        _renderer = renderer;
        _cultureInfoProvider = cultureInfoProvider;
    }

    public async Task UpdateAsync(
        string spreadsheetId,
        SubjectCoursePoints model,
        CancellationToken cancellationToken)
    {
        SheetId sheetId = await _sheetService.CreateOrClearSheetAsync(spreadsheetId, Title, cancellationToken);

        IComponent component = new PointsTable(_cultureInfoProvider).Render(model);

        var sheetInfo = new SheetInfo(spreadsheetId, sheetId.Value, Title);
        await _renderer.RenderAsync(component, sheetInfo, cancellationToken);
    }
}