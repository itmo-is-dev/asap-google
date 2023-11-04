using FluentSpreadsheets;
using FluentSpreadsheets.GoogleSheets.Factories;
using FluentSpreadsheets.GoogleSheets.Models;
using FluentSpreadsheets.GoogleSheets.Rendering;
using Itmo.Dev.Asap.Google.Application.Abstractions.Tables;
using Itmo.Dev.Asap.Google.Application.Models.Tables.PartialPoints;
using Itmo.Dev.Asap.Google.Common;
using Itmo.Dev.Asap.Google.Common.Tools;

namespace Itmo.Dev.Asap.Google.Application.PartialPoints;

public class PartialPointsTableWriter : ITableWriter<PartialSubjectCoursePoints>
{
    private const string Title = SheetConfigurations.Points.Title;

    private readonly ICultureInfoProvider _cultureInfoProvider;
    private readonly ISheetInfoFactory _sheetInfoFactory;
    private readonly IGoogleSheetsComponentRenderer _renderer;

    public PartialPointsTableWriter(
        ICultureInfoProvider cultureInfoProvider,
        ISheetInfoFactory sheetInfoFactory,
        IGoogleSheetsComponentRenderer renderer)
    {
        _cultureInfoProvider = cultureInfoProvider;
        _sheetInfoFactory = sheetInfoFactory;
        _renderer = renderer;
    }

    public async Task UpdateAsync(
        string spreadsheetId,
        PartialSubjectCoursePoints model,
        CancellationToken cancellationToken)
    {
        IComponent component = new PartialPointsTable(_cultureInfoProvider).Render(model);

        SheetInfo sheetInfo = await _sheetInfoFactory.GetAsync(spreadsheetId, Title, cancellationToken);
        await _renderer.RenderAsync(component, sheetInfo, cancellationToken);
    }
}