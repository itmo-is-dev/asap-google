using Itmo.Dev.Asap.Google.Application.DataAccess.Queries;
using Itmo.Dev.Asap.Google.Application.DataAccess.Repositories;
using Itmo.Dev.Asap.Google.Domain.SubjectCourses;
using Itmo.Dev.Platform.Postgres.Connection;
using Itmo.Dev.Platform.Postgres.Extensions;
using Itmo.Dev.Platform.Postgres.UnitOfWork;
using Npgsql;
using System.Runtime.CompilerServices;

namespace Itmo.Dev.Asap.Google.DataAccess.Repositories;

public class SubjectCourseRepository : ISubjectCourseRepository
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
        select subject_course_id, subject_course_spreadsheet_id from subject_courses
        where subject_course_id = :id
        """;

        NpgsqlConnection connection = await _connectionProvider.GetConnectionAsync(cancellationToken);

        await using NpgsqlCommand command = new NpgsqlCommand(sql, connection)
            .AddParameter("id", id);

        await using NpgsqlDataReader reader = await command.ExecuteReaderAsync(cancellationToken);

        int idOrdinal = reader.GetOrdinal("subject_course_id");
        int spreadsheetIdOrdinal = reader.GetOrdinal("subject_course_spreadsheet_id");

        if (await reader.ReadAsync(cancellationToken) is false)
            return null;

        return new GoogleSubjectCourse(reader.GetGuid(idOrdinal), reader.GetString(spreadsheetIdOrdinal));
    }

    public async IAsyncEnumerable<GoogleSubjectCourse> QueryAsync(
        SubjectCourseQuery query,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        const string sql = """
        select subject_course_id, subject_course_spreadsheet_id from subject_courses
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

        while (await reader.ReadAsync(cancellationToken))
        {
            yield return new GoogleSubjectCourse(
                reader.GetGuid(id),
                reader.GetString(spreadsheetId));
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