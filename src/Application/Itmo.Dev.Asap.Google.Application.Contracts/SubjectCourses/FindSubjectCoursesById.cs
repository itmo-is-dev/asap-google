using Itmo.Dev.Asap.Google.Application.Dto.SubjectCourses;
using MediatR;

namespace Itmo.Dev.Asap.Google.Application.Contracts.SubjectCourses;

internal static class FindSubjectCoursesById
{
    public record Query(IEnumerable<Guid> Ids) : IRequest<Response>;

    public record Response(IEnumerable<GoogleSubjectCourseDto> SubjectCourses);
}