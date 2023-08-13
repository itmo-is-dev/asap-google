using Itmo.Dev.Asap.Google.Application.Abstractions.Providers;
using System.Globalization;

namespace Itmo.Dev.Asap.Google.Application.Providers;

public class RuCultureInfoProvider : ICultureInfoProvider
{
    private static readonly CultureInfo RuCultureInfo = new CultureInfo("ru-RU");

    public CultureInfo GetCultureInfo()
    {
        return RuCultureInfo;
    }
}