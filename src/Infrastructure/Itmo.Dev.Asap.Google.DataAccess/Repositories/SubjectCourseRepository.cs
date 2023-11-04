using Itmo.Dev.Asap.Google.Application.Abstractions.DataAccess.Queries;
using Itmo.Dev.Asap.Google.Application.Abstractions.DataAccess.Repositories;
using Itmo.Dev.Asap.Google.Application.Models.SubjectCourses;
using Itmo.Dev.Platform.Postgres.Connection;
using Itmo.Dev.Platform.Postgres.Extensions;
using Itmo.Dev.Platform.Postgres.UnitOfWork;
using Npgsql;
using System.Runtime.CompilerServices;

namespace Itmo.Dev.Asap.Google.DataAccess.Repositories;

internal class SubjectCourseRepository : ISubjectCourseRepository
{
    private readonly IPostgresConnectionProvider _connectionProvider;
    private readonly IUnitOfWork _unitOfWork;

    public SubjectCourseRepository(IPostgresConnectionProvider connectionProvider, IUnitOfWork unitOfWork)
    {
        _connectionProvider = connectionProvider;
        _unitOfWork = unitOfWork;
    }

    public async Task<GoogleSubjectCourse?> FindByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        const string sql = """
        select subject_course_id, 
               subject_course_spreadsheet_id,
               (select count(*) from subject_courses ss where ss.subject_course_id = s.subject_course_id) as assignment_count
        from subject_courses s
        where subject_course_id = :id
        """;

        NpgsqlConnection connection = await _connectionProvider.GetConnectionAsync(cancellationToken);

        await using NpgsqlCommand command = new NpgsqlCommand(sql, connection)
            .AddParameter("id", id);

        await using NpgsqlDataReader reader = await command.ExecuteReaderAsync(cancellationToken);

        int idOrdinal = reader.GetOrdinal("subject_course_id");
        int spreadsheetIdOrdinal = reader.GetOrdinal("subject_course_spreadsheet_id");
        int assignmentCountOrdinal = reader.GetOrdinal("assignment_count");

        if (await reader.ReadAsync(cancellationToken) is false)
            return null;

        return new GoogleSubjectCourse(
            Id: reader.GetGuid(idOrdinal),
            SpreadsheetId: reader.GetString(spreadsheetIdOrdinal),
            AssignmentCount: reader.GetInt32(assignmentCountOrdinal));
    }

    public async IAsyncEnumerable<GoogleSubjectCourse> QueryAsync(
        SubjectCourseQuery query,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        const string sql = """
        select subject_course_id,
               subject_course_spreadsheet_id,
               (select count(*) from subject_courses ss where ss.subject_course_id = s.subject_course_id) as assignment_count
        from subject_courses s
        where 
            (cardinality(:ids) = 0 or subject_course_id = any(:ids))
            and (cardinality(:spreadsheet_ids) = 0 or subject_course_spreadsheet_id = any(:spreadsheet_ids))
        """;

        NpgsqlConnection connection = await _connectionProvider.GetConnectionAsync(cancellationToken);

        await using NpgsqlCommand command = new NpgsqlCommand(sql, connection)
            .AddParameter("ids", query.Ids)
            .AddParameter("spreadsheet_ids", query.SpreadsheetIds);

        await using NpgsqlDataReader reader = await command.ExecuteReaderAsync(cancellationToken);

        int id = reader.GetOrdinal("subject_course_id");
        int spreadsheetId = reader.GetOrdinal("subject_course_spreadsheet_id");
        int assignmentCount = reader.GetOrdinal("assignment_count");

        while (await reader.ReadAsync(cancellationToken))
        {
            yield return new GoogleSubjectCourse(
                Id: reader.GetGuid(id),
                SpreadsheetId: reader.GetString(spreadsheetId),
                AssignmentCount: reader.GetInt32(assignmentCount));
        }
    }

    public void Add(GoogleSubjectCourse course, CancellationToken cancellationToken)
    {
        const string sql = """
        insert into subject_courses
        (subject_course_id, subject_course_spreadsheet_id) values (:id, :spreadsheet_id)
        """;

        NpgsqlCommand command = new NpgsqlCommand(sql)
            .AddParameter("id", course.Id)
            .AddParameter("spreadsheet_id", course.SpreadsheetId);

        _unitOfWork.Enqueue(command);
    }
}