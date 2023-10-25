using FluentSpreadsheets;
using FluentSpreadsheets.Tables;
using Itmo.Dev.Asap.Google.Common.Tools;
using Itmo.Dev.Asap.Google.Domain.Students;
using Itmo.Dev.Asap.Google.Domain.SubjectCourses;
using Itmo.Dev.Asap.Google.Domain.Tables.Extensions;
using System.Drawing;
using System.Globalization;
using static FluentSpreadsheets.ComponentFactory;

namespace Itmo.Dev.Asap.Google.Domain.Tables;

public class LabsTable : RowTable<SubjectCoursePoints>
{
    private static readonly IComponent BlankLabel = Label(string.Empty);

    private static readonly IComponent EmptyAssignmentPointsCell = HStack(
        BlankLabel.WithLeadingMediumBorder(),
        BlankLabel.WithTrailingMediumBorder());

    private readonly ICultureInfoProvider _cultureInfoProvider;

    public LabsTable(ICultureInfoProvider cultureInfoProvider)
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
            SubjectCoursePoints.StudentPoints studentPoints = model.Students[i];
            Student student = studentPoints.Student;

            double totalPoints = studentPoints.Points.Sum(p => p.Value.Points);
            double roundedPoints = Math.Round(totalPoints, 2);

            IRowComponent row = Row(
                    Label(student.UniversityId),
                    Label(student.FullName),
                    Label(student.GroupName),
                    Label(studentPoints.GithubUserName ?? string.Empty),
                    ForEach(
                        model.Assignments,
                        a => BuildAssignmentPointsCell(a.Value, studentPoints.Points, currentCulture)),
                    Label(roundedPoints, currentCulture).WithTrailingMediumBorder())
                .WithDefaultStyle(i, model.Students.Count)
                .WithGroupSeparators(i, model);

            yield return row;
        }
    }

    private static IComponent BuildAssignmentPointsCell(
        SubjectCoursePoints.Assignment assignment,
        IReadOnlyDictionary<Guid, SubjectCoursePoints.AssignmentPoints> points,
        IFormatProvider formatProvider)
    {
        if (points.TryGetValue(assignment.Id, out SubjectCoursePoints.AssignmentPoints assignmentPoints) is false)
            return EmptyAssignmentPointsCell;

        IComponent stack = HStack(
            Label(assignmentPoints.Points, formatProvider).WithLeadingMediumBorder(),
            Label(assignmentPoints.Date, formatProvider).WithTrailingMediumBorder());

        if (assignmentPoints.IsBanned)
            stack = stack.FilledWith(Color.Red);

        return stack;
    }
}