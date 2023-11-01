using Itmo.Dev.Asap.Github.Models;
using Itmo.Dev.Asap.Google.Application.Abstractions.Github.Models;
using Itmo.Dev.Asap.Google.Common;

namespace Itmo.Dev.Asap.Google.Integrations.Github.Mapping;

public static class ProtoMapper
{
    public static GithubUserModel MapToGithubUserModel(this GithubUser user)
    {
        return new GithubUserModel(user.UserId.ToGuid(), user.Username);
    }
}