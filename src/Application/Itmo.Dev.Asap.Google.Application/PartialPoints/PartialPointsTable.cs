using FluentSpreadsheets;
using FluentSpreadsheets.Tables;
using Itmo.Dev.Asap.Google.Application.Extensions;
using Itmo.Dev.Asap.Google.Application.Models.Tables.PartialPoints;
using Itmo.Dev.Asap.Google.Common.Tools;
using System.Drawing;
using System.Globalization;
using static FluentSpreadsheets.ComponentFactory;

namespace Itmo.Dev.Asap.Google.Application.PartialPoints;

public class PartialPointsTable : RowTable<PartialSubjectCoursePoints>
{
    private static readonly IComponent[] EmptyStudentPrefix =
    {
        Empty(),
        Empty(),
        Empty(),
        Empty(),
    };

    private readonly ICultureInfoProvider _cultureInfoProvider;

    public PartialPointsTable(ICultureInfoProvider cultureInfoProvider)
    {
        _cultureInfoProvider = cultureInfoProvider;
    }

    protected override IEnumerable<IRowComponent> RenderRows(PartialSubjectCoursePoints model)
    {
        IComponentGroup emptyAssignmentsForEach = ForEach(
            Enumerable.Range(0, model.AssignmentCount),
            _ => VStack(Empty(), HStack(Empty(), Empty())));

        yield return Row(
            Empty(),
            Empty(),
            Empty(),
            Empty(),
            emptyAssignmentsForEach.CustomizedWith(g => VStack(Empty(), g)),
            Empty());

        int ordinal = 0;

        IRowComponent emptyRow = Row(
            Empty(),
            Empty(),
            Empty(),
            Empty(),
            ForEach(Enumerable.Range(0, model.AssignmentCount), _ => HStack(Empty(), Empty())),
            Empty());

        foreach (PartialStudentAssignmentPoints studentPoints in model.StudentPoints.OrderBy(x => x.StudentOrdinal))
        {
            while (ordinal < studentPoints.StudentOrdinal)
            {
                yield return emptyRow;
                ordinal++;
            }

            IEnumerable<IComponent> components = EmptyStudentPrefix
                .Concat(GetAssignmentPointsComponents(
                    studentPoints.StudentOrdinal,
                    model.AssignmentCount,
                    studentPoints.Points))
                .Append(Empty());

            yield return Row(components);
        }
    }

    private IEnumerable<IComponent> GetAssignmentPointsComponents(
        int studentOrdinal,
        int assignmentCount,
        IEnumerable<PartialAssignmentPoints> points)
    {
        int ordinal = 0;

        foreach (PartialAssignmentPoints point in points.OrderBy(x => x.AssignmentOrdinal))
        {
            while (ordinal < point.AssignmentOrdinal)
            {
                yield return HStack(Empty(), Empty());
                ordinal++;
            }

            CultureInfo formatProvider = _cultureInfoProvider.GetCultureInfo();

            IComponent stack = HStack(
                Label(point.AssignmentPoints.Points, formatProvider),
                Label(point.AssignmentPoints.Date, formatProvider));

            stack = point.AssignmentPoints.IsBanned
                ? stack.FilledWith(Color.Red)
                : stack.WithAlternatingColor(studentOrdinal);

            yield return stack;
            ordinal++;
        }

        while (ordinal < assignmentCount)
        {
            yield return HStack(Empty(), Empty());
            ordinal++;
        }
    }
}