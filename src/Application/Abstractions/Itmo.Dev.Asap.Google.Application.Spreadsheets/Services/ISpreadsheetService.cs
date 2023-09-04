using Itmo.Dev.Asap.Google.Application.Spreadsheets.Models;

namespace Itmo.Dev.Asap.Google.Application.Spreadsheets.Services;

public interface ISpreadsheetService
{
    Task<SpreadsheetCreateResult> CreateSpreadsheetAsync(string title, CancellationToken token);

    Task<GoogleSpreadsheet?> FindSpreadsheetAsync(string id, CancellationToken cancellationToken);
}