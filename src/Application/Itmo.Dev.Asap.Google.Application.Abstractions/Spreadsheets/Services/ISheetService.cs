using Itmo.Dev.Asap.Google.Application.Abstractions.Spreadsheets.Models;

namespace Itmo.Dev.Asap.Google.Application.Abstractions.Spreadsheets.Services;

public interface ISheetService
{
    Task<SheetId> CreateOrClearSheetAsync(string spreadsheetId, string sheetTitle, CancellationToken token);

    Task<SheetId> CreateSheetAsync(string spreadsheetId, string sheetTitle, CancellationToken token);
}