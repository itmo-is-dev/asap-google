using System.Text.RegularExpressions;

namespace Itmo.Dev.Asap.Google.Spreadsheets.Extensions;

internal static partial class StringExtensions
{
    public static bool HasCyrillic(this string s)
    {
        return HasCyrillicRegex().IsMatch(s);
    }

    [GeneratedRegex(@"\p{IsCyrillic}", RegexOptions.Compiled)]
    private static partial Regex HasCyrillicRegex();
}