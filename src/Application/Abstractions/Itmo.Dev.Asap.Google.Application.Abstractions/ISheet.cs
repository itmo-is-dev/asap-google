namespace Itmo.Dev.Asap.Google.Application.Abstractions;

public interface ISheet<in TModel>
{
    Task UpdateAsync(string spreadsheetId, TModel model, CancellationToken cancellationToken);
}