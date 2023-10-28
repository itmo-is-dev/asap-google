using Itmo.Dev.Asap.Google.Application.Abstractions.DataAccess;
using Itmo.Dev.Asap.Google.Application.Abstractions.DataAccess.Repositories;
using Itmo.Dev.Platform.Postgres.UnitOfWork;
using System.Data;

namespace Itmo.Dev.Asap.Google.DataAccess;

public class PersistenceContext : IPersistenceContext
{
    private readonly IUnitOfWork _unitOfWork;

    public PersistenceContext(
        IUnitOfWork unitOfWork,
        ISubjectCourseRepository subjectCourses,
        ISubjectCourseStudentRepository subjectCourseStudents,
        ISubjectCourseAssignmentRepository subjectCourseAssignments)
    {
        _unitOfWork = unitOfWork;
        SubjectCourses = subjectCourses;
        SubjectCourseStudents = subjectCourseStudents;
        SubjectCourseAssignments = subjectCourseAssignments;
    }

    public ISubjectCourseRepository SubjectCourses { get; }

    public ISubjectCourseStudentRepository SubjectCourseStudents { get; }

    public ISubjectCourseAssignmentRepository SubjectCourseAssignments { get; }

    public ValueTask CommitAsync(IsolationLevel isolationLevel, CancellationToken cancellationToken)
    {
        return _unitOfWork.CommitAsync(isolationLevel, cancellationToken);
    }

    public ValueTask CommitAsync(CancellationToken cancellationToken)
    {
        return _unitOfWork.CommitAsync(IsolationLevel.ReadCommitted, cancellationToken);
    }
}