using FluentSpreadsheets;
using FluentSpreadsheets.Tables;
using Itmo.Dev.Asap.Google.Application.Extensions;
using Itmo.Dev.Asap.Google.Application.Models.Tables.Points;
using Itmo.Dev.Asap.Google.Common.Tools;
using System.Drawing;
using System.Globalization;
using static FluentSpreadsheets.ComponentFactory;

namespace Itmo.Dev.Asap.Google.Application.Points;

public class PointsTable : RowTable<SubjectCoursePoints>
{
    private static readonly IComponent BlankLabel = Label(string.Empty);

    private static readonly IComponent EmptyAssignmentPointsCell = HStack(
        BlankLabel.WithLeadingMediumBorder(),
        BlankLabel.WithTrailingMediumBorder());

    private readonly ICultureInfoProvider _cultureInfoProvider;

    public PointsTable(ICultureInfoProvider cultureInfoProvider)
    {
        _cultureInfoProvider = cultureInfoProvider;
    }

    protected override IComponent Customize(IComponent component)
    {
        return component.WithDefaultStyle();
    }

    protected override IEnumerable<IRowComponent> RenderRows(SubjectCoursePoints model)
    {
        yield return Row(
            Label("ISU").WithColumnWidth(0.8),
            Label("ФИО").WithColumnWidth(2),
            Label("Группа"),
            Label("GitHub").WithColumnWidth(1.2).Frozen(),
            ForEach(
                    model.Assignments,
                    a => VStack(
                        Label(a.Value.Name).WithSideMediumBorder(),
                        HStack(
                            Label("Балл").WithLeadingMediumBorder(),
                            Label("Дата").WithTrailingMediumBorder())))
                .CustomizedWith(g =>
                    VStack(Label("Лабораторные").WithSideMediumBorder().WithBottomMediumBorder(), g)),
            Label("Итог").WithTrailingMediumBorder());

        CultureInfo currentCulture = _cultureInfoProvider.GetCultureInfo();

        for (int i = 0; i < model.Students.Count; i++)
        {
            PointsStudent student = model.Students[i];
            StudentAssignmentPoints studentAssignmentPoints = model.StudentPoints[student.Id];

            double totalPoints = studentAssignmentPoints.Points.Sum(p => p.Value.Points);
            double roundedPoints = Math.Round(totalPoints, 2);

            IRowComponent row = Row(
                    Label(student.UniversityId),
                    Label(student.FullName),
                    Label(student.GroupName),
                    Label(student.GithubUserName ?? string.Empty),
                    ForEach(
                        model.Assignments,
                        a => BuildAssignmentPointsCell(a.Value, studentAssignmentPoints.Points, currentCulture)),
                    Label(roundedPoints, currentCulture).WithTrailingMediumBorder())
                .WithDefaultStyle(i, model.Students.Count)
                .WithGroupSeparators(i, model);

            yield return row;
        }
    }

    private static IComponent BuildAssignmentPointsCell(
        Assignment assignment,
        IReadOnlyDictionary<Guid, AssignmentPoints> points,
        IFormatProvider formatProvider)
    {
        if (points.TryGetValue(assignment.Id, out AssignmentPoints? assignmentPoints) is false)
            return EmptyAssignmentPointsCell;

        IComponent stack = HStack(
            Label(assignmentPoints.Points, formatProvider).WithLeadingMediumBorder(),
            Label(assignmentPoints.Date, formatProvider).WithTrailingMediumBorder());

        if (assignmentPoints.IsBanned)
            stack = stack.FilledWith(Color.Red);

        return stack;
    }
}