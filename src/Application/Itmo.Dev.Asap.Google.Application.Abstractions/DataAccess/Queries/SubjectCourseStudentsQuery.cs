using SourceKit.Generators.Builder.Annotations;

namespace Itmo.Dev.Asap.Google.Application.Abstractions.DataAccess.Queries;

[GenerateBuilder]
public partial record SubjectCourseStudentsQuery(
    Guid[] SubjectCourseIds,
    Guid[] StudentIds);