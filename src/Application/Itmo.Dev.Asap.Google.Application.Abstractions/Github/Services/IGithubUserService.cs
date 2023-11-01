using Itmo.Dev.Asap.Google.Application.Abstractions.Github.Models;

namespace Itmo.Dev.Asap.Google.Application.Abstractions.Github.Services;

public interface IGithubUserService
{
    IAsyncEnumerable<GithubUserModel> FindByIdsAsync(IEnumerable<string> userIds, CancellationToken cancellationToken);
}