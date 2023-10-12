using Itmo.Dev.Asap.Google.Application.DataAccess.Queries;
using Itmo.Dev.Asap.Google.Domain.Assignments;

namespace Itmo.Dev.Asap.Google.Application.DataAccess.Repositories;

public interface ISubjectCourseAssignmentRepository
{
    IAsyncEnumerable<SubjectCourseAssignment> QueryAsync(
        SubjectCourseAssignmentsQuery query,
        CancellationToken cancellationToken);

    void AddOrUpdateRange(IReadOnlyCollection<SubjectCourseAssignment> assignments);
}