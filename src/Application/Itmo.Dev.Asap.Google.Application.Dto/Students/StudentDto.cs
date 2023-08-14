using Itmo.Dev.Asap.Google.Application.Dto.Users;

namespace Itmo.Dev.Asap.Google.Application.Dto.Students;

public record StudentDto(UserDto User, string GroupName);