using Itmo.Dev.Asap.Google.Common.Tools;
using System.Globalization;

namespace Itmo.Dev.Asap.Google.Application.Providers;

public class CultureInfoProvider : ICultureInfoProvider
{
    private static readonly CultureInfo CultureInfo;

    static CultureInfoProvider()
    {
        CultureInfo = new CultureInfo("ru-RU")
        {
            NumberFormat =
            {
                CurrencyDecimalSeparator = ".",
                CurrencyGroupSeparator = " ",
                NumberDecimalSeparator = ".",
                NumberGroupSeparator = " ",
            },
        };
    }

    public CultureInfo GetCultureInfo()
    {
        return CultureInfo;
    }
}