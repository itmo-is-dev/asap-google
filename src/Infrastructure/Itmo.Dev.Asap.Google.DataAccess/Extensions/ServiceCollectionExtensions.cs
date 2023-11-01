using Itmo.Dev.Asap.Google.Application.Abstractions.DataAccess;
using Itmo.Dev.Asap.Google.Application.Abstractions.DataAccess.Repositories;
using Itmo.Dev.Asap.Google.DataAccess.Repositories;
using Itmo.Dev.Platform.Postgres.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Itmo.Dev.Asap.Google.DataAccess.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDataAccess(this IServiceCollection collection)
    {
        collection.AddPlatformPostgres(builder => builder
            .BindConfiguration("Infrastructure:DataAccess:PostgresConfiguration"));

        collection.AddScoped<IPersistenceContext, PersistenceContext>();

        collection.AddScoped<ISubjectCourseRepository, SubjectCourseRepository>();
        collection.AddScoped<ISubjectCourseStudentRepository, SubjectCourseStudentRepository>();
        collection.AddScoped<ISubjectCourseAssignmentRepository, SubjectCourseAssignmentRepository>();

        collection.AddPlatformMigrations(typeof(IAssemblyMarker).Assembly);

        return collection;
    }

    public static Task UseDataAccessAsync(this IServiceScope scope, CancellationToken cancellationToken)
    {
        return scope.UsePlatformMigrationsAsync(cancellationToken);
    }
}