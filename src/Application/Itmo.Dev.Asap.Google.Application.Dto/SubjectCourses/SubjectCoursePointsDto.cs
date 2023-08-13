using Itmo.Dev.Asap.Google.Application.Dto.Students;

namespace Itmo.Dev.Asap.Google.Application.Dto.SubjectCourses;

public record struct SubjectCoursePointsDto(
    IReadOnlyCollection<AssignmentDto> Assignments,
    IReadOnlyList<StudentPointsDto> StudentPoints);