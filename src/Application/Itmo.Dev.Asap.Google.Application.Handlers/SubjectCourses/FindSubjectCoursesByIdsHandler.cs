using Itmo.Dev.Asap.Google.Application.DataAccess;
using Itmo.Dev.Asap.Google.Application.DataAccess.Queries;
using Itmo.Dev.Asap.Google.Application.Dto.SubjectCourses;
using Itmo.Dev.Asap.Google.Application.Mapping;
using MediatR;
using static Itmo.Dev.Asap.Google.Application.Contracts.SubjectCourses.FindSubjectCoursesById;

namespace Itmo.Dev.Asap.Google.Application.Handlers.SubjectCourses;

internal class FindSubjectCoursesByIdsHandler : IRequestHandler<Query, Response>
{
    private readonly IPersistenceContext _context;

    public FindSubjectCoursesByIdsHandler(IPersistenceContext context)
    {
        _context = context;
    }

    public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
    {
        var query = SubjectCourseQuery.Build(x => x.WithIds(request.Ids));

        GoogleSubjectCourseDto[] dto = await _context.SubjectCourses
            .QueryAsync(query, cancellationToken)
            .Select(x => x.ToDto())
            .ToArrayAsync(cancellationToken);

        return new Response(dto);
    }
}