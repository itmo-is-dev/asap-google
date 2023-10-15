using SourceKit.Generators.Builder.Annotations;

namespace Itmo.Dev.Asap.Google.Application.DataAccess.Queries;

[GenerateBuilder]
public partial record SubjectCourseAssignmentsQuery(
    Guid[] SubjectCourseIds,
    Guid[] AssignmentIds);