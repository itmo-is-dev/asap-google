using Itmo.Dev.Asap.Google.Application.Dto.SubjectCourses;
using Itmo.Dev.Asap.Google.Presentation.Kafka.Mappers;
using Itmo.Dev.Asap.Kafka;
using Itmo.Dev.Platform.Kafka.Consumer;
using Itmo.Dev.Platform.Kafka.Consumer.Models;
using Itmo.Dev.Platform.Kafka.Extensions;
using MediatR;
using static Itmo.Dev.Asap.Google.Application.Contracts.SubjectCourses.Notifications.SubjectCoursesCreated;

namespace Itmo.Dev.Asap.Google.Presentation.Kafka.Handlers;

public class SubjectCourseCreatedHandler : IKafkaMessageHandler<SubjectCourseCreatedKey, SubjectCourseCreatedValue>
{
    private readonly IMediator _mediator;

    public SubjectCourseCreatedHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async ValueTask HandleAsync(
        IEnumerable<ConsumerKafkaMessage<SubjectCourseCreatedKey, SubjectCourseCreatedValue>> messages,
        CancellationToken cancellationToken)
    {
        IEnumerable<SubjectCourseDto> subjectCourses = messages
            .GetLatestByKey()
            .Select(x => x.Value.SubjectCourse.MapTo());

        var notification = new Notification(subjectCourses);
        await _mediator.Publish(notification, cancellationToken);
    }
}