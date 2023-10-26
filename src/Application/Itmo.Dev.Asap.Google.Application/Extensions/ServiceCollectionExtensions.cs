using FluentSpreadsheets;
using FluentSpreadsheets.GoogleSheets.Extensions;
using Itmo.Dev.Asap.Google.Application.Abstractions;
using Itmo.Dev.Asap.Google.Application.Dto.SubjectCourses;
using Itmo.Dev.Asap.Google.Application.Dto.Submissions;
using Itmo.Dev.Asap.Google.Application.Providers;
using Itmo.Dev.Asap.Google.Application.TableWriters;
using Itmo.Dev.Asap.Google.Common.Tools;
using Microsoft.Extensions.DependencyInjection;

namespace Itmo.Dev.Asap.Google.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGoogleApplication(this IServiceCollection collection)
    {
        collection
            .AddSingleton<ITableWriter<SubjectCoursePointsDto>, LabsTableWriter>()
            .AddSingleton<ITableWriter<SubmissionsQueueDto>, QueueTableWriter>();

        collection
            .AddFluentSpreadsheets()
            .AddGoogleSheets();

        collection.AddSingleton<ICultureInfoProvider, RuCultureInfoProvider>();

        return collection;
    }
}