using Itmo.Dev.Asap.Google.Application.DataAccess.Queries;
using Itmo.Dev.Asap.Google.Domain.SubjectCourses;

namespace Itmo.Dev.Asap.Google.Application.DataAccess.Repositories;

public interface ISubjectCourseRepository
{
    Task<GoogleSubjectCourse?> FindByIdAsync(Guid id, CancellationToken cancellationToken);

    IAsyncEnumerable<GoogleSubjectCourse> QueryAsync(SubjectCourseQuery query, CancellationToken cancellationToken);

    void Add(GoogleSubjectCourse course, CancellationToken cancellationToken);
}