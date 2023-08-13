using Grpc.Core;
using Itmo.Dev.Asap.Google.Application.Contracts.SubjectCourses;
using Itmo.Dev.Asap.Google.Presentation.Grpc.Mapping;
using MediatR;

namespace Itmo.Dev.Asap.Google.Presentation.Grpc.Services;

public class GoogleSubjectCourseController : Google.GoogleSubjectCourseService.GoogleSubjectCourseServiceBase
{
    private readonly IMediator _mediator;

    public GoogleSubjectCourseController(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override async Task<FindByIdsResponse> FindByIds(FindByIdsRequest request, ServerCallContext context)
    {
        FindSubjectCoursesById.Query query = request.MapToApplicationRequest();
        FindSubjectCoursesById.Response response = await _mediator.Send(query, context.CancellationToken);

        return response.MapToGrpcResponse();
    }
}