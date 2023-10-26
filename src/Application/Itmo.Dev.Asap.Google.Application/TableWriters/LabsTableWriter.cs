using FluentSpreadsheets;
using FluentSpreadsheets.GoogleSheets.Models;
using FluentSpreadsheets.GoogleSheets.Rendering;
using Itmo.Dev.Asap.Google.Application.Abstractions;
using Itmo.Dev.Asap.Google.Application.Dto.SubjectCourses;
using Itmo.Dev.Asap.Google.Application.Spreadsheets.Models;
using Itmo.Dev.Asap.Google.Application.Spreadsheets.Services;
using Itmo.Dev.Asap.Google.Common;
using Itmo.Dev.Asap.Google.Common.Collections;
using Itmo.Dev.Asap.Google.Common.Tools;
using Itmo.Dev.Asap.Google.Domain.Students;
using Itmo.Dev.Asap.Google.Domain.SubjectCourses;
using Itmo.Dev.Asap.Google.Domain.Tables;

namespace Itmo.Dev.Asap.Google.Application.TableWriters;

public class LabsTableWriter : ITableWriter<SubjectCoursePointsDto>
{
    private const string Title = SheetConfigurations.Labs.Title;

    private readonly IGoogleSheetsComponentRenderer _renderer;
    private readonly ISheetService _sheetService;
    private readonly ICultureInfoProvider _cultureInfoProvider;

    public LabsTableWriter(
        ISheetService sheetService,
        IGoogleSheetsComponentRenderer renderer,
        ICultureInfoProvider cultureInfoProvider)
    {
        _sheetService = sheetService;
        _renderer = renderer;
        _cultureInfoProvider = cultureInfoProvider;
    }

    public async Task UpdateAsync(
        string spreadsheetId,
        SubjectCoursePointsDto model,
        CancellationToken cancellationToken)
    {
        SheetId sheetId = await _sheetService.CreateOrClearSheetAsync(spreadsheetId, Title, cancellationToken);

        IComponent component = new LabsTable(_cultureInfoProvider).Render(Map(model));

        var sheetInfo = new SheetInfo(spreadsheetId, sheetId.Value, Title);
        await _renderer.RenderAsync(component, sheetInfo, cancellationToken);
    }

    private static SubjectCoursePoints Map(SubjectCoursePointsDto model)
    {
        IReadOnlyList<SubjectCoursePoints.Student> students = CollectionFactory.CreateList(model.Students, Map);

        IReadOnlyDictionary<Guid, SubjectCoursePoints.Assignment> assignments = CollectionFactory.CreateDictionary(
            model.Assignments,
            static x => new SubjectCoursePoints.Assignment(x.Id, x.ShortName));

        IReadOnlyDictionary<Guid, SubjectCoursePoints.StudentAssignmentPoints> points = CollectionFactory
            .CreateDictionary(model.StudentPoints, Map);

        return new SubjectCoursePoints(students, assignments, points);
    }

    private static SubjectCoursePoints.Student Map(SubjectCoursePointsDto.StudentDto dto)
    {
        var student = new Student(
            dto.User.Id,
            dto.User.FirstName,
            dto.User.MiddleName,
            dto.User.LastName,
            dto.User.UniversityId,
            dto.GroupName);

        return new SubjectCoursePoints.Student(student, dto.GithubUsername);
    }

    private static SubjectCoursePoints.StudentAssignmentPoints Map(SubjectCoursePointsDto.StudentPointsDto dto)
    {
        IReadOnlyDictionary<Guid, SubjectCoursePoints.AssignmentPoints> points = CollectionFactory.CreateDictionary(
            dto.Points,
            static x => new SubjectCoursePoints.AssignmentPoints(x.AssignmentId, x.Date, x.IsBanned, x.Points));

        return new SubjectCoursePoints.StudentAssignmentPoints(dto.StudentId, points);
    }
}