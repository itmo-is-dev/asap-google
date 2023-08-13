using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Itmo.Dev.Asap.Google.Application.Spreadsheets.Services;
using Itmo.Dev.Asap.Google.Spreadsheets.Services;
using Itmo.Dev.Asap.Google.Spreadsheets.Tools;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Itmo.Dev.Asap.Google.Spreadsheets.Extensions;

public static class ServiceCollectionExtensions
{
    private static readonly string[] AccessScopes = { SheetsService.Scope.Spreadsheets, DriveService.Scope.Drive };

    public static IServiceCollection AddGoogleInfrastructureIntegration(this IServiceCollection collection)
    {
        collection
            .AddOptions<GoogleIntegrationConfiguration>()
            .BindConfiguration("Infrastructure:Spreadsheets");

        collection.AddSingleton<ISpreadsheetService, SpreadsheetService>();
        collection.AddSingleton<ISheetService, SheetService>();

        collection.AddSingleton<BaseClientService.Initializer>(p =>
        {
            IOptions<GoogleIntegrationConfiguration> options = p
                .GetRequiredService<IOptions<GoogleIntegrationConfiguration>>();

            var credential = GoogleCredential.FromJson(options.Value.ClientSecrets);

            return new BaseClientService.Initializer
            {
                HttpClientInitializer = credential.CreateScoped(AccessScopes),
            };
        });

        collection.AddSingleton<SheetsService>(
            p => new SheetsService(p.GetRequiredService<BaseClientService.Initializer>()));

        collection.AddSingleton<DriveService>(
            p => new DriveService(p.GetRequiredService<BaseClientService.Initializer>()));

        collection.AddSingleton<DriveParentProvider>(p =>
        {
            IOptions<GoogleIntegrationConfiguration> options = p
                .GetRequiredService<IOptions<GoogleIntegrationConfiguration>>();

            return new DriveParentProvider(options.Value.GoogleDriveId);
        });

        return collection;
    }
}