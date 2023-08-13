using Itmo.Dev.Asap.Google.Application.Dto.Users;

namespace Itmo.Dev.Asap.Google.Application.Formatters;

public interface IUserFullNameFormatter
{
    string GetFullName(UserDto user);
}