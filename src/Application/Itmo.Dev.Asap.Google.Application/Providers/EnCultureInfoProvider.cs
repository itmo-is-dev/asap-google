using Itmo.Dev.Asap.Google.Common.Tools;
using System.Globalization;

namespace Itmo.Dev.Asap.Google.Application.Providers;

public class EnCultureInfoProvider : ICultureInfoProvider
{
    private static readonly CultureInfo EnCultureInfo = new CultureInfo("en-US");

    public CultureInfo GetCultureInfo()
    {
        return EnCultureInfo;
    }
}