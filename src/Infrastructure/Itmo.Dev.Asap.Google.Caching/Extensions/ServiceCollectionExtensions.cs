using Itmo.Dev.Asap.Google.Caching.Models;
using Itmo.Dev.Asap.Google.Caching.Tools;
using Itmo.Dev.Asap.Google.Common.Tools;
using Microsoft.Extensions.DependencyInjection;

namespace Itmo.Dev.Asap.Google.Caching.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGoogleCaching(this IServiceCollection collection)
    {
        collection.AddOptions<GithubCacheConfiguration>().BindConfiguration("Infrastructure:Cache");
        collection.AddSingleton<IGoogleMemoryCache, GoogleMemoryCache>();

        return collection;
    }
}