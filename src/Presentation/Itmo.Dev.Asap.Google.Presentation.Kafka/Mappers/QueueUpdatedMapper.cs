using Google.Protobuf.WellKnownTypes;
using Itmo.Dev.Asap.Google.Application.Dto.Students;
using Itmo.Dev.Asap.Google.Application.Dto.Submissions;
using Itmo.Dev.Asap.Kafka;
using Riok.Mapperly.Abstractions;
using static Itmo.Dev.Asap.Google.Application.Contracts.Queues.Notifications.QueueUpdated;

namespace Itmo.Dev.Asap.Google.Presentation.Kafka.Mappers;

[Mapper(EnumMappingStrategy = EnumMappingStrategy.ByName)]
public static partial class QueueUpdatedMapper
{
    public static Notification MapTo(this QueueUpdatedValue message)
    {
        IReadOnlyDictionary<Guid, StudentDto> students = MapStudents(message.SubmissionsQueue.Students);
        IReadOnlyList<SubmissionDto> submissions = MapSubmissions(message.SubmissionsQueue.Submissions);

        var queue = new SubmissionsQueueDto(message.StudentGroupName, students, submissions);

        return new Notification(
            Guid.Parse(message.SubjectCourseId),
            Guid.Parse(message.StudentGroupId),
            queue);
    }

    private static partial IReadOnlyDictionary<Guid, StudentDto> MapStudents(
        IReadOnlyDictionary<string, QueueUpdatedValue.Types.Student> students);

    private static partial IReadOnlyList<SubmissionDto> MapSubmissions(
        IReadOnlyCollection<QueueUpdatedValue.Types.Submission> submissions);

    private static DateTime ToDateTime(Timestamp timestamp)
        => timestamp.ToDateTime();
}