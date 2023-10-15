using Index = FluentSpreadsheets.Index;

namespace Itmo.Dev.Asap.Google.Application.Extensions;

internal static class IndexExtensions
{
    public static Index WithRowShift(this Index index, int shift)
    {
        return index with { Row = index.Row + shift };
    }

    public static Index WithColumnShift(this Index index, int shift)
    {
        return index with { Column = index.Column + shift };
    }
}