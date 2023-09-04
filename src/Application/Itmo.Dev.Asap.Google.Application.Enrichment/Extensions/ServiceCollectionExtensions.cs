using Itmo.Dev.Asap.Google.Application.Abstractions.Enrichment;
using Microsoft.Extensions.DependencyInjection;

namespace Itmo.Dev.Asap.Google.Application.Enrichment.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEnrichment(this IServiceCollection collection)
    {
        collection.AddScoped<IGoogleSubjectCourseEnricher, GoogleSubjectCourseEnricher>();
        return collection;
    }
}