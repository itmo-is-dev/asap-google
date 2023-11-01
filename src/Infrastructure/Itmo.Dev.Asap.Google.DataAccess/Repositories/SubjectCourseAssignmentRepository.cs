using Itmo.Dev.Asap.Google.Application.Abstractions.DataAccess.Queries;
using Itmo.Dev.Asap.Google.Application.Abstractions.DataAccess.Repositories;
using Itmo.Dev.Asap.Google.Application.Models.SubjectCourses;
using Itmo.Dev.Platform.Postgres.Connection;
using Itmo.Dev.Platform.Postgres.Extensions;
using Itmo.Dev.Platform.Postgres.UnitOfWork;
using Npgsql;
using System.Runtime.CompilerServices;

namespace Itmo.Dev.Asap.Google.DataAccess.Repositories;

internal class SubjectCourseAssignmentRepository : ISubjectCourseAssignmentRepository
{
    private readonly IPostgresConnectionProvider _connectionProvider;
    private readonly IUnitOfWork _unitOfWork;

    public SubjectCourseAssignmentRepository(IPostgresConnectionProvider connectionProvider, IUnitOfWork unitOfWork)
    {
        _connectionProvider = connectionProvider;
        _unitOfWork = unitOfWork;
    }

    public async IAsyncEnumerable<SubjectCourseAssignment> QueryAsync(
        SubjectCourseAssignmentsQuery query,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        const string sql = """
        select subject_course_id, assignment_id, subject_course_assignment_ordinal
        from subject_course_assignments
        where 
            (cardinality(:subject_course_ids) = 0 or subject_course_id = any(:subject_course_id))
            and (cardinality(:assignment_ids) = 0 or assignment_id = any(:assignment_ids));
        """;

        NpgsqlConnection connection = await _connectionProvider.GetConnectionAsync(cancellationToken);

        await using NpgsqlCommand command = new NpgsqlCommand(sql, connection)
            .AddParameter("subject_course_ids", query.SubjectCourseIds)
            .AddParameter("assignment_ids", query.AssignmentIds);

        await using NpgsqlDataReader reader = await command.ExecuteReaderAsync(cancellationToken);

        int subjectCourseId = reader.GetOrdinal("subject_course_id");
        int assignmentId = reader.GetOrdinal("assignment_id");
        int ordinal = reader.GetOrdinal("subject_course_assignment_ordinal");

        while (await reader.ReadAsync(cancellationToken))
        {
            yield return new SubjectCourseAssignment(
                SubjectCourseId: reader.GetGuid(subjectCourseId),
                AssignmentId: reader.GetGuid(assignmentId),
                Ordinal: reader.GetInt32(ordinal));
        }
    }

    public void AddOrUpdateRange(IReadOnlyCollection<SubjectCourseAssignment> assignments)
    {
        const string sql = """
        insert into subject_course_assignments(subject_course_id, assignment_id, subject_course_assignment_ordinal) 
        select * from unnest(:subject_course_ids, :assignment_ids, :ordinals)
        on conflict on constraint subject_course_assignments_pkey
        do update 
        set subject_course_assignment_ordinal = excluded.subject_course_assignment_ordinal;
        """;

        NpgsqlCommand command = new NpgsqlCommand(sql)
            .AddParameter("subject_course_ids", assignments.Select(x => x.SubjectCourseId).ToArray())
            .AddParameter("assignment_ids", assignments.Select(x => x.AssignmentId).ToArray())
            .AddParameter("ordinals", assignments.Select(x => x.Ordinal).ToArray());

        _unitOfWork.Enqueue(command);
    }
}