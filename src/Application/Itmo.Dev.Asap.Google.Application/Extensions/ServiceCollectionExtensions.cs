using FluentSpreadsheets;
using FluentSpreadsheets.GoogleSheets.Extensions;
using Itmo.Dev.Asap.Google.Application.Abstractions.Enrichment;
using Itmo.Dev.Asap.Google.Application.Abstractions.Tables;
using Itmo.Dev.Asap.Google.Application.Enrichment;
using Itmo.Dev.Asap.Google.Application.Models.Tables.Points;
using Itmo.Dev.Asap.Google.Application.Models.Tables.Queues;
using Itmo.Dev.Asap.Google.Application.Points;
using Itmo.Dev.Asap.Google.Application.Providers;
using Itmo.Dev.Asap.Google.Application.Queues;
using Itmo.Dev.Asap.Google.Common.Tools;
using Microsoft.Extensions.DependencyInjection;

namespace Itmo.Dev.Asap.Google.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGoogleApplication(this IServiceCollection collection)
    {
        collection
            .AddSingleton<ITableWriter<SubjectCoursePoints>, PointsTableWriter>()
            .AddSingleton<ITableWriter<Queue>, QueueTableWriter>();

        collection
            .AddFluentSpreadsheets()
            .AddGoogleSheets();

        collection.AddSingleton<ICultureInfoProvider, RuCultureInfoProvider>();

        collection.AddMediatR(x => x.RegisterServicesFromAssemblyContaining<IAssemblyMarker>());

        collection.AddScoped<IGoogleSubjectCourseEnricher, GoogleSubjectCourseEnricher>();

        return collection;
    }
}