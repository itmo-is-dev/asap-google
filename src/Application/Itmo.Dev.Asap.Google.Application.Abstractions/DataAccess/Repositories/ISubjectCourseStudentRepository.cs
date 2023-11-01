using Itmo.Dev.Asap.Google.Application.Abstractions.DataAccess.Queries;
using Itmo.Dev.Asap.Google.Application.Models.SubjectCourses;

namespace Itmo.Dev.Asap.Google.Application.Abstractions.DataAccess.Repositories;

public interface ISubjectCourseStudentRepository
{
    IAsyncEnumerable<SubjectCourseStudent> QueryAsync(
        SubjectCourseStudentsQuery query,
        CancellationToken cancellationToken);

    void AddOrUpdateRange(IReadOnlyCollection<SubjectCourseStudent> students);
}