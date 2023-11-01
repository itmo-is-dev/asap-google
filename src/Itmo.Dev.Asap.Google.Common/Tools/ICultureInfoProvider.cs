using System.Globalization;

namespace Itmo.Dev.Asap.Google.Common.Tools;

public interface ICultureInfoProvider
{
    CultureInfo GetCultureInfo();
}