using Itmo.Dev.Asap.Google.Application.DataAccess.Queries;
using Itmo.Dev.Asap.Google.Application.DataAccess.Repositories;
using Itmo.Dev.Asap.Google.Domain.Students;
using Itmo.Dev.Platform.Postgres.Connection;
using Itmo.Dev.Platform.Postgres.Extensions;
using Itmo.Dev.Platform.Postgres.UnitOfWork;
using Npgsql;
using System.Runtime.CompilerServices;

namespace Itmo.Dev.Asap.Google.DataAccess.Repositories;

internal class SubjectCourseStudentRepository : ISubjectCourseStudentRepository
{
    private readonly IPostgresConnectionProvider _connectionProvider;
    private readonly IUnitOfWork _unitOfWork;

    public SubjectCourseStudentRepository(IPostgresConnectionProvider connectionProvider, IUnitOfWork unitOfWork)
    {
        _connectionProvider = connectionProvider;
        _unitOfWork = unitOfWork;
    }

    public async IAsyncEnumerable<SubjectCourseStudent> QueryAsync(
        SubjectCourseStudentsQuery query,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        const string sql = """
        select student_id, subject_course_id, subject_course_student_ordinal
        from subject_course_students
        where 
            (cardinality(:student_ids) = 0 or student_id = any(:student_ids))
            and (cardinality(:subject_course_ids) = 0 or subject_course_id = any(:subject_course_ids));
        """;

        NpgsqlConnection connection = await _connectionProvider.GetConnectionAsync(cancellationToken);

        await using NpgsqlCommand command = new NpgsqlCommand(sql, connection)
            .AddParameter("student_ids", query.StudentIds)
            .AddParameter("subject_course_ids", query.SubjectCourseIds);

        await using NpgsqlDataReader reader = await command.ExecuteReaderAsync(cancellationToken);

        int studentId = reader.GetOrdinal("student_id");
        int subjectCourseId = reader.GetOrdinal("subject_course_id");
        int ordinal = reader.GetOrdinal("subject_course_student_ordinal");

        while (await reader.ReadAsync(cancellationToken))
        {
            yield return new SubjectCourseStudent(
                StudentId: reader.GetGuid(studentId),
                SubjectCourseId: reader.GetGuid(subjectCourseId),
                Ordinal: reader.GetInt32(ordinal));
        }
    }

    public void AddOrUpdateRange(IReadOnlyCollection<SubjectCourseStudent> students)
    {
        const string sql = """
        insert into subject_course_students(student_id, subject_course_id, subject_course_student_ordinal) 
        select * from unnest(:student_ids, :subject_course_ids, :ordinals)
        on conflict on constraint subject_course_students_pkey
        do update 
        set subject_course_student_ordinal = excluded.subject_course_student_ordinal;
        """;

        NpgsqlCommand command = new NpgsqlCommand(sql)
            .AddParameter("student_ids", students.Select(x => x.StudentId).ToArray())
            .AddParameter("subject_course_ids", students.Select(x => x.SubjectCourseId).ToArray())
            .AddParameter("ordinals", students.Select(x => x.Ordinal).ToArray());

        _unitOfWork.Enqueue(command);
    }
}