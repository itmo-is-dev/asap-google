using Itmo.Dev.Asap.Github.Models;
using Itmo.Dev.Asap.Google.Application.Github.Models;
using Riok.Mapperly.Abstractions;

namespace Itmo.Dev.Asap.Google.Integrations.Github.Mapping;

[Mapper]
public static partial class ProtoMapper
{
    [MapProperty(nameof(GithubUser.UserId), nameof(GithubUserDto.Id))]
    [MapProperty(nameof(GithubUser.Username), nameof(GithubUserDto.GithubUsername))]
    public static partial GithubUserDto ToDto(this GithubUser user);
}