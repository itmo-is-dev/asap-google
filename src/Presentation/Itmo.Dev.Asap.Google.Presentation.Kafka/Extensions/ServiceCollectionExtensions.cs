using Itmo.Dev.Asap.Google.Presentation.Kafka.Handlers;
using Itmo.Dev.Asap.Kafka;
using Itmo.Dev.Platform.Kafka.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Itmo.Dev.Asap.Google.Presentation.Kafka.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddKafkaPresentation(
        this IServiceCollection collection,
        IConfiguration configuration)
    {
        const string queueUpdatedSectionKey = "Presentation:Kafka:Consumers:QueueUpdated";
        const string subjectCourseCreatedSectionKey = "Presentation:Kafka:Consumers:SubjectCourseCreated";
        const string subjectCoursePointsUpdatedSectionKey = "Presentation:Kafka:Consumers:SubjectCoursePointsUpdated";

        string host = configuration.GetSection("Presentation:Kafka:Host").Get<string>() ?? string.Empty;
        string group = Assembly.GetExecutingAssembly().GetName().Name ?? string.Empty;

        collection.AddKafkaConsumer<QueueUpdatedKey, QueueUpdatedValue>(builder => builder
            .HandleWith<QueueUpdatedHandler>()
            .DeserializeKeyWithProto()
            .DeserializeValueWithProto()
            .UseNamedOptionsConfiguration(
                "QueueUpdated",
                configuration.GetSection(queueUpdatedSectionKey),
                c => c.WithHost(host).WithGroup(group)));

        collection.AddKafkaConsumer<SubjectCourseCreatedKey, SubjectCourseCreatedValue>(builder => builder
            .HandleWith<SubjectCourseCreatedHandler>()
            .DeserializeKeyWithProto()
            .DeserializeValueWithProto()
            .UseNamedOptionsConfiguration(
                "SubjectCourseCreated",
                configuration.GetSection(subjectCourseCreatedSectionKey),
                c => c.WithHost(host).WithGroup(group)));

        collection.AddKafkaConsumer<SubjectCoursePointsUpdatedKey, SubjectCoursePointsUpdatedValue>(builder => builder
            .HandleWith<SubjectCoursePointsUpdatedHandler>()
            .DeserializeKeyWithProto()
            .DeserializeValueWithProto()
            .UseNamedOptionsConfiguration(
                "SubjectCoursePointsUpdated",
                configuration.GetSection(subjectCoursePointsUpdatedSectionKey),
                c => c.WithHost(host).WithGroup(group)));

        return collection;
    }
}