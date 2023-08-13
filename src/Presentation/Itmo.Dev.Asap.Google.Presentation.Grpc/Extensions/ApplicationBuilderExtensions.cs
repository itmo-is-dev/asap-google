using Itmo.Dev.Asap.Google.Presentation.Grpc.Services;
using Microsoft.AspNetCore.Builder;

namespace Itmo.Dev.Asap.Google.Presentation.Grpc.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseRpcPresentation(this IApplicationBuilder builder)
    {
        builder.UseEndpoints(x =>
        {
            x.MapGrpcService<GoogleSubjectCourseController>();
            x.MapGrpcReflectionService();
        });

        return builder;
    }
}

// using Itmo.Dev.Asap.Google.Application.Extensions;
// using Itmo.Dev.Asap.Google.Application.Handlers.Extensions;
// using Itmo.Dev.Asap.Google.DataAccess.Extensions;
// using Itmo.Dev.Asap.Google.Presentation.Services.Extensions;
// using Itmo.Dev.Asap.Google.Spreadsheets.Extensions;
// using Microsoft.Extensions.Configuration;
// using Microsoft.Extensions.DependencyInjection;
// using Npgsql;
//
// namespace Itmo.Dev.Asap.Google;
//
// public static class ServiceCollectionExtensions
// {
//     public static IServiceCollection AddAsapGoogle(
//         this IServiceCollection collection,
//         IConfiguration configuration,
//         string databaseConnectionString)
//     {
//         bool enabled = configuration.GetValue<bool>("Google:Enabled");
//
//         if (enabled)
//         {
//             collection
//                 .AddGoogleApplication()
//                 .AddGoogleApplicationHandlers()
//                 .AddGoogleInfrastructureIntegration(configuration)
//                 .AddGooglePresentationServices();
//
//             collection.AddGoogleDataAccess(_ => new NpgsqlConnection(databaseConnectionString));
//         }
//         else
//         {
//             collection.AddDummyGooglePresentationServices();
//         }
//
//         return collection;
//     }
// }