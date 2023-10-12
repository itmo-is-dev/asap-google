using FluentSpreadsheets;
using FluentSpreadsheets.GoogleSheets.Extensions;
using FluentSpreadsheets.Tables;
using Itmo.Dev.Asap.Google.Application.Abstractions;
using Itmo.Dev.Asap.Google.Application.Abstractions.Models;
using Itmo.Dev.Asap.Google.Application.Abstractions.Providers;
using Itmo.Dev.Asap.Google.Application.Dto.SubjectCourses;
using Itmo.Dev.Asap.Google.Application.Dto.Submissions;
using Itmo.Dev.Asap.Google.Application.Formatters;
using Itmo.Dev.Asap.Google.Application.Providers;
using Itmo.Dev.Asap.Google.Application.Sheets;
using Itmo.Dev.Asap.Google.Application.Tables;
using Microsoft.Extensions.DependencyInjection;

namespace Itmo.Dev.Asap.Google.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGoogleApplication(this IServiceCollection collection)
    {
        collection
            .AddSingleton<ISheet<CourseStudentsDto>, PointsSheet>()
            .AddSingleton<ISheet<SubjectCoursePointsDto>, LabsSheet>()
            .AddSingleton<ISheet<SubmissionsQueueDto>, QueueSheet>();

        collection
            .AddSingleton<ITable<CourseStudentsDto>, PointsTable>()
            .AddSingleton<ITable<SubjectCoursePointsDto>, LabsTable>()
            .AddSingleton<ITable<SubmissionsQueueDto>, QueueTable>();

        collection
            .AddFluentSpreadsheets()
            .AddGoogleSheets();

        collection
            .AddSingleton<IUserFullNameFormatter, UserFullNameFormatter>()
            .AddSingleton<ICultureInfoProvider, RuCultureInfoProvider>();

        return collection;
    }
}