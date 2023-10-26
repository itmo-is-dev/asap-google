using FluentSpreadsheets;
using FluentSpreadsheets.GoogleSheets.Models;
using FluentSpreadsheets.GoogleSheets.Rendering;
using Itmo.Dev.Asap.Google.Application.Abstractions;
using Itmo.Dev.Asap.Google.Application.Dto.Submissions;
using Itmo.Dev.Asap.Google.Application.Spreadsheets.Models;
using Itmo.Dev.Asap.Google.Application.Spreadsheets.Services;
using Itmo.Dev.Asap.Google.Common.Collections;
using Itmo.Dev.Asap.Google.Common.Tools;
using Itmo.Dev.Asap.Google.Domain.Queues;
using Itmo.Dev.Asap.Google.Domain.Students;
using Itmo.Dev.Asap.Google.Domain.Tables;

namespace Itmo.Dev.Asap.Google.Application.TableWriters;

public class QueueTableWriter : ITableWriter<SubmissionsQueueDto>
{
    private readonly IGoogleSheetsComponentRenderer _renderer;
    private readonly ISheetService _sheetService;
    private readonly ICultureInfoProvider _cultureInfoProvider;

    public QueueTableWriter(
        ISheetService sheetService,
        IGoogleSheetsComponentRenderer renderer,
        ICultureInfoProvider cultureInfoProvider)
    {
        _sheetService = sheetService;
        _renderer = renderer;
        _cultureInfoProvider = cultureInfoProvider;
    }

    public async Task UpdateAsync(string spreadsheetId, SubmissionsQueueDto model, CancellationToken cancellationToken)
    {
        string title = model.GroupName;

        SheetId sheetId = await _sheetService.CreateOrClearSheetAsync(spreadsheetId, title, cancellationToken);

        IComponent component = new QueueTable(_cultureInfoProvider).Render(Map(model));

        var sheetInfo = new SheetInfo(spreadsheetId, sheetId.Value, title);
        await _renderer.RenderAsync(component, sheetInfo, cancellationToken);
    }

    private static SubmissionQueue Map(SubmissionsQueueDto model)
    {
        IReadOnlyDictionary<Guid, Student> students = CollectionFactory.CreateDictionary(
            model.Students,
            x => new Student(
                x.User.Id,
                x.User.FirstName,
                x.User.MiddleName,
                x.User.LastName,
                x.User.UniversityId,
                x.GroupName));

        IReadOnlyList<Submission> submissions = CollectionFactory.CreateList(
            model.Submissions,
            x => new Submission(
                x.Id,
                x.StudentId,
                x.SubmissionDate,
                x.Payload,
                x.AssignmentShortName,
                Map(x.State),
                x.Code));

        return new SubmissionQueue(students, submissions);
    }

    private static SubmissionState Map(SubmissionStateDto state)
    {
        return state switch
        {
            SubmissionStateDto.Active => SubmissionState.Active,
            SubmissionStateDto.Inactive => SubmissionState.Inactive,
            SubmissionStateDto.Deleted => SubmissionState.Deleted,
            SubmissionStateDto.Completed => SubmissionState.Completed,
            SubmissionStateDto.Reviewed => SubmissionState.Reviewed,
            SubmissionStateDto.Banned => SubmissionState.Banned,
            _ => throw new ArgumentOutOfRangeException(nameof(state), state, null),
        };
    }
}