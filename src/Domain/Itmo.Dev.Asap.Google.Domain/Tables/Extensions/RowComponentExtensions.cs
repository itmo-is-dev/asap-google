using FluentSpreadsheets;
using FluentSpreadsheets.Styles;
using Itmo.Dev.Asap.Google.Domain.SubjectCourses;
using System.Drawing;

namespace Itmo.Dev.Asap.Google.Domain.Tables.Extensions;

internal static class RowComponentExtensions
{
    public static IRowComponent FilledWith(this IRowComponent row, int alpha, Color baseColor)
    {
        return row.FilledWith(Color.FromArgb(alpha, baseColor));
    }

    public static IRowComponent WithTopMediumBorder(this IRowComponent row)
    {
        return row.WithTopBorderType(BorderType.Medium);
    }

    public static IRowComponent WithBottomMediumBorder(this IRowComponent row)
    {
        return row.WithBottomBorderType(BorderType.Medium);
    }

    public static IRowComponent WithDefaultStyle(this IRowComponent row, int rowNumber, int maxRowNumber)
    {
        row = row.WithAlternatingColor(rowNumber);

        if (rowNumber is 0)
            row = row.WithTopMediumBorder();

        if (rowNumber == maxRowNumber - 1)
            row = row.WithBottomMediumBorder();

        return row;
    }

    public static IRowComponent WithGroupSeparators(
        this IRowComponent row,
        int rowNumber,
        SubjectCoursePoints points)
    {
        if (rowNumber is 0)
            return row;

        SubjectCoursePoints.Student student1 = points.Students[rowNumber];
        SubjectCoursePoints.Student student2 = points.Students[rowNumber - 1];

        if (student1.Value.GroupName != student2.Value.GroupName)
            row = row.WithTopMediumBorder();

        return row;
    }

    private static IRowComponent WithAlternatingColor(this IRowComponent row, int rowNumber)
    {
        return (rowNumber % 2) switch
        {
            0 => row.FilledWith(Color.AliceBlue),
            _ => row.FilledWith(Color.Transparent),
        };
    }
}