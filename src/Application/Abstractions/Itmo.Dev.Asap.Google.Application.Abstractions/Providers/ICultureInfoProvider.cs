using System.Globalization;

namespace Itmo.Dev.Asap.Google.Application.Abstractions.Providers;

public interface ICultureInfoProvider
{
    CultureInfo GetCultureInfo();
}