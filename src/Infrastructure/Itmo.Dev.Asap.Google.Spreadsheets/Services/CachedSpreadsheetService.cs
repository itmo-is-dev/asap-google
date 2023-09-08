using Itmo.Dev.Asap.Google.Application.Spreadsheets.Models;
using Itmo.Dev.Asap.Google.Application.Spreadsheets.Services;
using Itmo.Dev.Asap.Google.Common.Tools;
using Microsoft.Extensions.Caching.Memory;

namespace Itmo.Dev.Asap.Google.Spreadsheets.Services;

public class CachedSpreadsheetService : ISpreadsheetService
{
    private readonly IGoogleMemoryCache _cache;
    private readonly ISpreadsheetService _service;

    public CachedSpreadsheetService(IGoogleMemoryCache cache, ISpreadsheetService service)
    {
        _cache = cache;
        _service = service;
    }

    public Task<SpreadsheetCreateResult> CreateSpreadsheetAsync(string title, CancellationToken cancellationToken)
    {
        return _service.CreateSpreadsheetAsync(title, cancellationToken);
    }

    public Task<GoogleSpreadsheet?> FindSpreadsheetAsync(string id, CancellationToken cancellationToken)
    {
        return _cache.GetOrCreateAsync(
            (nameof(CachedSpreadsheetService), nameof(FindSpreadsheetAsync), id),
            _ => _service.FindSpreadsheetAsync(id, cancellationToken));
    }
}