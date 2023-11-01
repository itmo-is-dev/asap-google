using Itmo.Dev.Asap.Google.Application.Abstractions.DataAccess.Queries;
using Itmo.Dev.Asap.Google.Application.Models.SubjectCourses;

namespace Itmo.Dev.Asap.Google.Application.Abstractions.DataAccess.Repositories;

public interface ISubjectCourseRepository
{
    Task<GoogleSubjectCourse?> FindByIdAsync(Guid id, CancellationToken cancellationToken);

    IAsyncEnumerable<GoogleSubjectCourse> QueryAsync(SubjectCourseQuery query, CancellationToken cancellationToken);

    void Add(GoogleSubjectCourse course, CancellationToken cancellationToken);
}