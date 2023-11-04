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
        const string keyPath = "Presentation:Kafka:Consumers";

        string host = configuration.GetSection("Presentation:Kafka:Host").Get<string>() ?? string.Empty;
        string group = Assembly.GetExecutingAssembly().GetName().Name ?? string.Empty;

        collection.AddKafkaConsumer<QueueUpdatedKey, QueueUpdatedValue>(builder => builder
            .HandleWith<QueueUpdatedHandler>()
            .DeserializeKeyWithProto()
            .DeserializeValueWithProto()
            .UseNamedOptionsConfiguration(
                "QueueUpdated",
                configuration.GetSection($"{keyPath}:QueueUpdated"),
                c => c.WithHost(host).WithGroup(group)));

        collection.AddKafkaConsumer<SubjectCourseCreatedKey, SubjectCourseCreatedValue>(builder => builder
            .HandleWith<SubjectCourseCreatedHandler>()
            .DeserializeKeyWithProto()
            .DeserializeValueWithProto()
            .UseNamedOptionsConfiguration(
                "SubjectCourseCreated",
                configuration.GetSection($"{keyPath}:SubjectCourseCreated"),
                c => c.WithHost(host).WithGroup(group)));

        collection.AddKafkaConsumer<SubjectCoursePointsUpdatedKey, SubjectCoursePointsUpdatedValue>(builder => builder
            .HandleWith<SubjectCoursePointsUpdatedHandler>()
            .DeserializeKeyWithProto()
            .DeserializeValueWithProto()
            .UseNamedOptionsConfiguration(
                "SubjectCoursePointsUpdated",
                configuration.GetSection($"{keyPath}:SubjectCoursePointsUpdated"),
                c => c.WithHost(host).WithGroup(group)));

        collection.AddKafkaConsumer<StudentPointsUpdatedKey, StudentPointsUpdatedValue>(builder => builder
            .HandleWith<StudentPointsUpdatedHandler>()
            .DeserializeKeyWithProto()
            .DeserializeValueWithProto()
            .UseNamedOptionsConfiguration(
                "StudentPointsUpdated",
                configuration.GetSection($"{keyPath}:StudentPointsUpdated"),
                c => c.WithHost(host).WithGroup(group)));

        return collection;
    }
}