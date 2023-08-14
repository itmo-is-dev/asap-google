using Itmo.Dev.Asap.Github.Models;
using Itmo.Dev.Asap.Github.Users;
using Itmo.Dev.Asap.Google.Application.Github.Models;
using Itmo.Dev.Asap.Google.Application.Github.Services;
using Itmo.Dev.Asap.Google.Integrations.Github.Mapping;
using System.Runtime.CompilerServices;

namespace Itmo.Dev.Asap.Google.Integrations.Github.Services;

public class GithubUserService : IGithubUserService
{
    private readonly Asap.Github.Users.GithubUserService.GithubUserServiceClient _client;

    public GithubUserService(Asap.Github.Users.GithubUserService.GithubUserServiceClient client)
    {
        _client = client;
    }

    public async IAsyncEnumerable<GithubUserDto> FindByIdsAsync(
        IEnumerable<string> userIds,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var request = new FindByIdsRequest { UserIds = { userIds } };
        FindByIdsResponse response = await _client.FindByIdsAsync(request, cancellationToken: cancellationToken);

        foreach (GithubUser user in response.Users)
        {
            yield return user.ToDto();
        }
    }
}