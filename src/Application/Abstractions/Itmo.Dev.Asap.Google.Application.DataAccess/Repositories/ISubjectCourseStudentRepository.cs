using Itmo.Dev.Asap.Google.Application.DataAccess.Queries;
using Itmo.Dev.Asap.Google.Domain.Students;

namespace Itmo.Dev.Asap.Google.Application.DataAccess.Repositories;

public interface ISubjectCourseStudentRepository
{
    IAsyncEnumerable<SubjectCourseStudent> QueryAsync(
        SubjectCourseStudentsQuery query,
        CancellationToken cancellationToken);

    void AddOrUpdateRange(IReadOnlyCollection<SubjectCourseStudent> students);
}