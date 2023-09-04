using Itmo.Dev.Asap.Google.Application.Dto.SubjectCourses;
using Itmo.Dev.Asap.Google.Domain.SubjectCourses;

namespace Itmo.Dev.Asap.Google.Application.Abstractions.Enrichment;

public interface IGoogleSubjectCourseEnricher
{
    Task<GoogleSubjectCourseDto> EnrichAsync(GoogleSubjectCourse subjectCourse, CancellationToken cancellationToken);
}