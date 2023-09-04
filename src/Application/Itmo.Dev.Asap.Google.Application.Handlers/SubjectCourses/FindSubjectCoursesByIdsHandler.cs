using Itmo.Dev.Asap.Google.Application.Abstractions.Enrichment;
using Itmo.Dev.Asap.Google.Application.DataAccess;
using Itmo.Dev.Asap.Google.Application.DataAccess.Queries;
using Itmo.Dev.Asap.Google.Application.Dto.SubjectCourses;
using MediatR;
using static Itmo.Dev.Asap.Google.Application.Contracts.SubjectCourses.FindSubjectCoursesById;

namespace Itmo.Dev.Asap.Google.Application.Handlers.SubjectCourses;

internal class FindSubjectCoursesByIdsHandler : IRequestHandler<Query, Response>
{
    private readonly IPersistenceContext _context;
    private readonly IGoogleSubjectCourseEnricher _enricher;

    public FindSubjectCoursesByIdsHandler(IPersistenceContext context, IGoogleSubjectCourseEnricher enricher)
    {
        _context = context;
        _enricher = enricher;
    }

    public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
    {
        var query = SubjectCourseQuery.Build(x => x.WithIds(request.Ids));

        GoogleSubjectCourseDto[] dto = await _context.SubjectCourses
            .QueryAsync(query, cancellationToken)
            .SelectAwait(async x => await _enricher.EnrichAsync(x, cancellationToken))
            .ToArrayAsync(cancellationToken);

        return new Response(dto);
    }
}