using Itmo.Dev.Asap.Google.Presentation.Kafka.Mappers;
using Itmo.Dev.Asap.Kafka;
using Itmo.Dev.Platform.Kafka.Consumer;
using Itmo.Dev.Platform.Kafka.Consumer.Models;
using Itmo.Dev.Platform.Kafka.Extensions;
using MediatR;
using static Itmo.Dev.Asap.Google.Application.Contracts.Queues.Notifications.QueueUpdated;

namespace Itmo.Dev.Asap.Google.Presentation.Kafka.Handlers;

public class QueueUpdatedHandler : IKafkaMessageHandler<QueueUpdatedKey, QueueUpdatedValue>
{
    private readonly IMediator _mediator;

    public QueueUpdatedHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async ValueTask HandleAsync(
        IEnumerable<ConsumerKafkaMessage<QueueUpdatedKey, QueueUpdatedValue>> messages,
        CancellationToken cancellationToken)
    {
        IEnumerable<Notification> notifications = messages
            .GetLatestByKey()
            .Select(x => x.Value.MapTo());

        foreach (Notification notification in notifications)
        {
            await _mediator.Publish(notification, cancellationToken);
        }
    }
}