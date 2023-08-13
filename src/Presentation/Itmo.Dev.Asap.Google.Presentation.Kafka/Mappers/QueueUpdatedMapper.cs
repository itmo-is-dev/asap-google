using Google.Protobuf.WellKnownTypes;
using Itmo.Dev.Asap.Kafka;
using Riok.Mapperly.Abstractions;
using static Itmo.Dev.Asap.Google.Application.Contracts.Queues.Notifications.QueueUpdated;

namespace Itmo.Dev.Asap.Google.Presentation.Kafka.Mappers;

[Mapper(EnumMappingStrategy = EnumMappingStrategy.ByName)]
public static partial class QueueUpdatedMapper
{
    public static partial Notification MapTo(this QueueUpdatedValue message);

    private static DateTime ToDateTime(Timestamp timestamp)
        => timestamp.ToDateTime();
}