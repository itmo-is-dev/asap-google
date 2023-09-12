using Itmo.Dev.Asap.Google.Application.Github.Models;
using Itmo.Dev.Asap.Google.Application.Github.Services;
using Itmo.Dev.Asap.Google.Presentation.Kafka.Mappers;
using Itmo.Dev.Asap.Kafka;
using Itmo.Dev.Platform.Kafka.Consumer;
using Itmo.Dev.Platform.Kafka.Consumer.Models;
using Itmo.Dev.Platform.Kafka.Extensions;
using MediatR;
using Microsoft.Extensions.Logging;
using static Itmo.Dev.Asap.Google.Application.Contracts.SubjectCourses.Notifications.SubjectCoursePointsUpdated;

namespace Itmo.Dev.Asap.Google.Presentation.Kafka.Handlers;

public class SubjectCoursePointsUpdatedHandler
    : IKafkaMessageHandler<SubjectCoursePointsUpdatedKey, SubjectCoursePointsUpdatedValue>
{
    private readonly IMediator _mediator;
    private readonly IGithubUserService _githubUserService;
    private readonly ILogger<SubjectCoursePointsUpdatedHandler> _logger;

    public SubjectCoursePointsUpdatedHandler(
        IMediator mediator,
        IGithubUserService githubUserService,
        ILogger<SubjectCoursePointsUpdatedHandler> logger)
    {
        _mediator = mediator;
        _githubUserService = githubUserService;
        _logger = logger;
    }

    public async ValueTask HandleAsync(
        IEnumerable<ConsumerKafkaMessage<SubjectCoursePointsUpdatedKey, SubjectCoursePointsUpdatedValue>> messages,
        CancellationToken cancellationToken)
    {
        ConsumerKafkaMessage<SubjectCoursePointsUpdatedKey, SubjectCoursePointsUpdatedValue>[] latest = messages
            .GetLatestByKey()
            .ToArray();

        IEnumerable<string> studentIds = latest
            .SelectMany(x => x.Value.Points.Students)
            .Select(x => x.User.Id);

        Dictionary<Guid, GithubUserDto> githubUsers = await _githubUserService
            .FindByIdsAsync(studentIds, cancellationToken)
            .ToDictionaryAsync(x => x.Id, cancellationToken);

        IEnumerable<Notification> notifications = latest
            .Select(x => x.Value.MapTo(githubUsers));

        foreach (Notification notification in notifications)
        {
            _logger.LogInformation("Publishing notification = {Notification}", notification);
            await _mediator.Publish(notification, cancellationToken);
        }
    }
}