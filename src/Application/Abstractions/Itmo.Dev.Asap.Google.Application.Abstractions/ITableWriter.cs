namespace Itmo.Dev.Asap.Google.Application.Abstractions;

public interface ITableWriter<in TModel>
{
    Task UpdateAsync(string spreadsheetId, TModel model, CancellationToken cancellationToken);
}