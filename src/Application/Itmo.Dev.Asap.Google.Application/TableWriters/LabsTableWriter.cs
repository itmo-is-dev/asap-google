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
    public const string Title = SheetConfigurations.Labs.Title;

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
        var assignments = new DynamicReadonlyDictionary<Guid, AssignmentDto, SubjectCoursePoints.Assignment>(
            model.Assignments,
            x => new SubjectCoursePoints.Assignment(x.Id, x.ShortName));

        SubjectCoursePoints.StudentPoints[] studentPoints = model.StudentPoints.Select(Map).ToArray();

        return new SubjectCoursePoints(assignments, studentPoints);
    }

    private static SubjectCoursePoints.StudentPoints Map(SubjectCoursePointsDto.StudentPointsDto dto)
    {
        var student = new Student(
            dto.Student.User.Id,
            dto.Student.User.FirstName,
            dto.Student.User.MiddleName,
            dto.Student.User.LastName,
            dto.Student.User.UniversityId,
            dto.Student.GroupName);

        IReadOnlyDictionary<Guid, SubjectCoursePoints.AssignmentPoints> points = CollectionFactory.CreateDictionary(
            dto.Points,
            x => new SubjectCoursePoints.AssignmentPoints(x.AssignmentId, x.Date, x.IsBanned, x.Points));

        return new SubjectCoursePoints.StudentPoints(student, dto.Student.GithubUsername, points);
    }
}