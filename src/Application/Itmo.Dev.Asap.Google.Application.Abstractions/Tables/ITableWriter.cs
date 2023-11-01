namespace Itmo.Dev.Asap.Google.Application.Abstractions.Tables;

public interface ITableWriter<in TModel>
{
    Task UpdateAsync(string spreadsheetId, TModel model, CancellationToken cancellationToken);
}