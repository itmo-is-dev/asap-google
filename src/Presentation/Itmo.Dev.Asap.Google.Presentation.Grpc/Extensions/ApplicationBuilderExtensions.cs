using Itmo.Dev.Asap.Google.Presentation.Grpc.Services;
using Microsoft.AspNetCore.Builder;
using Prometheus;

namespace Itmo.Dev.Asap.Google.Presentation.Grpc.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseRpcPresentation(this IApplicationBuilder builder)
    {
        builder.UseEndpoints(x =>
        {
            x.MapGrpcService<GoogleSubjectCourseController>();
            x.MapGrpcReflectionService();
            x.MapMetrics();
        });

        return builder;
    }
}