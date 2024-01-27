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
        const string consumerKey = "Presentation:Kafka:Consumers";

        string group = Assembly.GetExecutingAssembly().GetName().Name ?? string.Empty;

        collection.AddKafka(builder => builder
            .ConfigureOptions(b => b.BindConfiguration("Presentation:Kafka"))
            .AddConsumer<QueueUpdatedKey, QueueUpdatedValue>(selector => selector
                .HandleWith<QueueUpdatedHandler>()
                .DeserializeKeyWithProto()
                .DeserializeValueWithProto()
                .UseNamedOptionsConfiguration(
                    "QueueUpdated",
                    configuration.GetSection($"{consumerKey}:QueueUpdated"),
                    c => c.WithGroup(group)))
            .AddConsumer<SubjectCourseCreatedKey, SubjectCourseCreatedValue>(selector => selector
                .HandleWith<SubjectCourseCreatedHandler>()
                .DeserializeKeyWithProto()
                .DeserializeValueWithProto()
                .UseNamedOptionsConfiguration(
                    "SubjectCourseCreated",
                    configuration.GetSection($"{consumerKey}:SubjectCourseCreated"),
                    c => c.WithGroup(group)))
            .AddConsumer<SubjectCoursePointsUpdatedKey, SubjectCoursePointsUpdatedValue>(selector => selector
                .HandleWith<SubjectCoursePointsUpdatedHandler>()
                .DeserializeKeyWithProto()
                .DeserializeValueWithProto()
                .UseNamedOptionsConfiguration(
                    "SubjectCoursePointsUpdated",
                    configuration.GetSection($"{consumerKey}:SubjectCoursePointsUpdated"),
                    c => c.WithGroup(group)))
            .AddConsumer<StudentPointsUpdatedKey, StudentPointsUpdatedValue>(selector => selector
                .HandleWith<StudentPointsUpdatedHandler>()
                .DeserializeKeyWithProto()
                .DeserializeValueWithProto()
                .UseNamedOptionsConfiguration(
                    "StudentPointsUpdated",
                    configuration.GetSection($"{consumerKey}:StudentPointsUpdated"),
                    c => c.WithGroup(group))));

        return collection;
    }
}