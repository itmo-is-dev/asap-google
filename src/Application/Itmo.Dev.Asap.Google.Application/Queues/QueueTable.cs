using FluentSpreadsheets;
using FluentSpreadsheets.Tables;
using Itmo.Dev.Asap.Google.Application.Extensions;
using Itmo.Dev.Asap.Google.Application.Models.Submissions;
using Itmo.Dev.Asap.Google.Application.Models.Tables.Queues;
using Itmo.Dev.Asap.Google.Common.Tools;
using System.Drawing;
using static FluentSpreadsheets.ComponentFactory;

namespace Itmo.Dev.Asap.Google.Application.Queues;

public class QueueTable : RowTable<Queue>
{
    private static readonly IRowComponent Header = Row(
        Label("ФИО").WithColumnWidth(2).WithFrozenRows(),
        Label("Группа"),
        Label("Лабораторная работа").WithColumnWidth(1.2),
        Label("Код"),
        Label("Дата").WithColumnWidth(1.2),
        Label("Статус"),
        Label("GitHub").WithColumnWidth(3.2).WithTrailingMediumBorder());

    private readonly ICultureInfoProvider _cultureInfoProvider;

    public QueueTable(ICultureInfoProvider cultureInfoProvider)
    {
        _cultureInfoProvider = cultureInfoProvider;
    }

    protected override IComponent Customize(IComponent component)
    {
        return component.WithDefaultStyle();
    }

    protected override IEnumerable<IRowComponent> RenderRows(Queue model)
    {
        yield return Header;

        IReadOnlyList<QueueSubmission> submissions = model.Submissions;

        for (int i = 0; i < submissions.Count; i++)
        {
            QueueSubmission submission = submissions[i];
            QueueStudent student = model.Students[submission.StudentId];

            IRowComponent row = Row(
                    Label(student.FullName),
                    Label(student.GroupName),
                    Label(submission.AssignmentShortName),
                    Label(submission.Code),
                    Label(submission.SubmissionDate, _cultureInfoProvider.GetCultureInfo()),
                    Label(submission.State.ToString()),
                    Label(submission.Payload).WithTrailingMediumBorder())
                .WithDefaultStyle(i, submissions.Count);

            if (submission.State is SubmissionState.Reviewed)
                row = row.FilledWith(125, Color.LightGreen);

            yield return row;
        }
    }
}