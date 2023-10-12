using Itmo.Dev.Asap.Google.Application.DataAccess.Repositories;
using System.Data;

namespace Itmo.Dev.Asap.Google.Application.DataAccess;

public interface IPersistenceContext
{
    ISubjectCourseRepository SubjectCourses { get; }

    ISubjectCourseStudentRepository SubjectCourseStudents { get; }

    ISubjectCourseAssignmentRepository SubjectCourseAssignments { get; }

    ValueTask CommitAsync(IsolationLevel isolationLevel, CancellationToken cancellationToken);

    ValueTask CommitAsync(CancellationToken cancellationToken);
}