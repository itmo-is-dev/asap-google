using FluentSpreadsheets;
using FluentSpreadsheets.Styles;
using Itmo.Dev.Asap.Google.Application.Abstractions.Models;
using Itmo.Dev.Asap.Google.Application.Dto.SubjectCourses;
using System.Drawing;

namespace Itmo.Dev.Asap.Google.Application.Extensions;

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
        SubjectCoursePointsDto points)
    {
        if (rowNumber is 0)
            return row;

        Guid student1Id = points.StudentPoints[rowNumber].StudentId;
        Guid student2Id = points.StudentPoints[rowNumber - 1].StudentId;

        SubjectCoursePointsDto.StudentDto student1 = points.Students[student1Id];
        SubjectCoursePointsDto.StudentDto student2 = points.Students[student2Id];

        if (student1.GroupName != student2.GroupName)
            row = row.WithTopMediumBorder();

        return row;
    }

    public static IRowComponent WithGroupSeparators(
        this IRowComponent row,
        int rowNumber,
        CourseStudentsDto points)
    {
        if (rowNumber is 0)
            return row;

        CourseStudentsDto.StudentDto student1 = points.Students[rowNumber];
        CourseStudentsDto.StudentDto student2 = points.Students[rowNumber - 1];

        if (student1.GroupName != student2.GroupName)
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