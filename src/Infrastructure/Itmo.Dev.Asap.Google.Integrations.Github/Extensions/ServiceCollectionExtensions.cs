using Itmo.Dev.Asap.Github.Users;
using Itmo.Dev.Asap.Google.Application.Github.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Itmo.Dev.Asap.Google.Integrations.Github.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGithubIntegration(this IServiceCollection collection)
    {
        collection.AddGrpcClient<GithubUserService.GithubUserServiceClient>();
        collection.AddScoped<IGithubUserService, Asap.Google.Integrations.Github.Services.GithubUserService>();

        return collection;
    }
}