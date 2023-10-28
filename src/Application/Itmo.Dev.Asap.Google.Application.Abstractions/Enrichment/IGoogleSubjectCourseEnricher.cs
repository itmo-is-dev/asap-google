using Itmo.Dev.Asap.Google.Application.Models.SubjectCourses;

namespace Itmo.Dev.Asap.Google.Application.Abstractions.Enrichment;

public interface IGoogleSubjectCourseEnricher
{
    Task<GoogleSubjectCourseDto> EnrichAsync(GoogleSubjectCourse subjectCourse, CancellationToken cancellationToken);
}