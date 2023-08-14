using Itmo.Dev.Asap.Google.Application.Github.Services;
using Itmo.Dev.Asap.Google.Integrations.Github.Services;
using Itmo.Dev.Asap.Google.Integrations.Github.Tools;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Itmo.Dev.Asap.Google.Integrations.Github.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGithubIntegration(this IServiceCollection collection)
    {
        collection
            .AddOptions<GithubIntegrationOptions>()
            .BindConfiguration("Infrastructure:Integrations:Github");

        collection.AddGrpcClient<Asap.Github.Users.GithubUserService.GithubUserServiceClient>((sp, o) =>
        {
            IOptions<GithubIntegrationOptions> options = sp.GetRequiredService<IOptions<GithubIntegrationOptions>>();
            o.Address = options.Value.ServiceUri;
        });

        collection.AddScoped<IGithubUserService, GithubUserService>();

        return collection;
    }
}