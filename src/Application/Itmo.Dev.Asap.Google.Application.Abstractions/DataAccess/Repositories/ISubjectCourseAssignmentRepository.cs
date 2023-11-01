using Itmo.Dev.Asap.Google.Application.Abstractions.DataAccess.Queries;
using Itmo.Dev.Asap.Google.Application.Models.SubjectCourses;

namespace Itmo.Dev.Asap.Google.Application.Abstractions.DataAccess.Repositories;

public interface ISubjectCourseAssignmentRepository
{
    IAsyncEnumerable<SubjectCourseAssignment> QueryAsync(
        SubjectCourseAssignmentsQuery query,
        CancellationToken cancellationToken);

    void AddOrUpdateRange(IReadOnlyCollection<SubjectCourseAssignment> assignments);
}