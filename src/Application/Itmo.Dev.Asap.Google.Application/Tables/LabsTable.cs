using FluentSpreadsheets;
using FluentSpreadsheets.Tables;
using Itmo.Dev.Asap.Google.Application.Abstractions.Providers;
using Itmo.Dev.Asap.Google.Application.Dto.SubjectCourses;
using Itmo.Dev.Asap.Google.Application.Extensions;
using Itmo.Dev.Asap.Google.Application.Formatters;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Globalization;
using static FluentSpreadsheets.ComponentFactory;

namespace Itmo.Dev.Asap.Google.Application.Tables;

[SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1110:Opening parenthesis or bracket should be on declaration line", Justification = "FluentSpreadsheets components setup readability is messy with this rule applied")]
[SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1111:Closing parenthesis should be on line of last parameter", Justification = "FluentSpreadsheets components setup readability is messy with this rule applied")]
[SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1009:Closing parenthesis should be spaced correctly", Justification = "FluentSpreadsheets components setup readability is messy with this rule applied")]
public class LabsTable : RowTable<SubjectCoursePointsDto>
{
    private static readonly IComponent BlankLabel = Label(string.Empty);

    private static readonly IComponent EmptyAssignmentPointsCell = HStack
    (
        BlankLabel.WithLeadingMediumBorder(),
        BlankLabel.WithTrailingMediumBorder()
    );

    private readonly ICultureInfoProvider _cultureInfoProvider;

    private readonly IUserFullNameFormatter _userFullNameFormatter;

    public LabsTable(IUserFullNameFormatter userFullNameFormatter, ICultureInfoProvider cultureInfoProvider)
    {
        _userFullNameFormatter = userFullNameFormatter;
        _cultureInfoProvider = cultureInfoProvider;
    }

    protected override IComponent Customize(IComponent component)
    {
        return component.WithDefaultStyle();
    }

    protected override IEnumerable<IRowComponent> RenderRows(SubjectCoursePointsDto model)
    {
        yield return Row
        (
            Label("ISU").WithColumnWidth(0.8),
            Label("ФИО").WithColumnWidth(2),
            Label("Группа"),
            Label("GitHub").WithColumnWidth(1.2).Frozen(),
            ForEach(model.Assignments, a => VStack
            (
                Label(a.Value.ShortName).WithSideMediumBorder(),
                HStack
                (
                    Label("Балл").WithLeadingMediumBorder(),
                    Label("Дата").WithTrailingMediumBorder()
                )
            )).CustomizedWith(g =>
                VStack(Label("Лабораторные").WithSideMediumBorder().WithBottomMediumBorder(), g)),
            Label("Итог").WithTrailingMediumBorder()
        );

        CultureInfo currentCulture = _cultureInfoProvider.GetCultureInfo();

        IReadOnlyList<SubjectCoursePointsDto.StudentPointsDto> studentPointsList = model.StudentPoints;

        for (int i = 0; i < studentPointsList.Count; i++)
        {
            SubjectCoursePointsDto.StudentPointsDto studentPoints = studentPointsList[i];
            SubjectCoursePointsDto.StudentDto student = model.Students[studentPoints.StudentId];

            double totalPoints = studentPoints.Points.Sum(p => p.Points);
            double roundedPoints = Math.Round(totalPoints, 2);

            IRowComponent row = Row
            (
                Label(student.User.UniversityId),
                Label(_userFullNameFormatter.GetFullName(student.User)),
                Label(student.GroupName),
                Label(student.GithubUsername ?? string.Empty),
                ForEach(
                    model.Assignments,
                    a => BuildAssignmentPointsCell(a.Value, studentPoints.Points, currentCulture)),
                Label(roundedPoints, currentCulture).WithTrailingMediumBorder()
            ).WithDefaultStyle(i, studentPointsList.Count).WithGroupSeparators(i, model);

            yield return row;
        }
    }

    private static IComponent BuildAssignmentPointsCell(
        AssignmentDto assignment,
        IEnumerable<SubjectCoursePointsDto.StudentPointsDto.AssignmentPointsDto> points,
        IFormatProvider formatProvider)
    {
        SubjectCoursePointsDto.StudentPointsDto.AssignmentPointsDto? assignmentPoints = points.FirstOrDefault(p => p.AssignmentId == assignment.Id);

        if (assignmentPoints is null)
            return EmptyAssignmentPointsCell;

        IComponent stack = HStack
        (
            Label(assignmentPoints.Points, formatProvider).WithLeadingMediumBorder(),
            Label(assignmentPoints.Date, formatProvider).WithTrailingMediumBorder()
        );

        if (assignmentPoints.IsBanned)
            stack = stack.FilledWith(Color.Red);

        return stack;
    }
}