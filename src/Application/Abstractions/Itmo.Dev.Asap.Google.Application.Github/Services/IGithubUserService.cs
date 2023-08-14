using Itmo.Dev.Asap.Google.Application.Github.Models;

namespace Itmo.Dev.Asap.Google.Application.Github.Services;

public interface IGithubUserService
{
    IAsyncEnumerable<GithubUserDto> FindByIdsAsync(IEnumerable<string> userIds, CancellationToken cancellationToken);
}