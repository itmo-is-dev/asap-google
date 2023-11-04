using Itmo.Dev.Asap.Google.Application.Abstractions.DataAccess.Queries;
using Itmo.Dev.Asap.Google.Application.Abstractions.DataAccess.Repositories;
using Itmo.Dev.Asap.Google.Application.Models.SubjectCourses;

namespace Itmo.Dev.Asap.Google.Application.SubjectCourses;

public static class SubjectCourseSpecifications
{
    public static async Task<GoogleSubjectCourse?> FindByIdAsync(
        this ISubjectCourseRepository repository,
        Guid id,
        CancellationToken cancellationToken)
    {
        var query = SubjectCourseQuery.Build(x => x.WithId(id));
        return await repository.QueryAsync(query, cancellationToken).FirstOrDefaultAsync(cancellationToken);
    }
}