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

        collection.AddPlatformKafka(builder => builder
            .ConfigureOptions(configuration.GetSection("Presentation:Kafka"))
            .AddConsumer(selector => selector
                .WithKey<QueueUpdatedKey>()
                .WithValue<QueueUpdatedValue>()
                .WithConfiguration(
                    configuration.GetSection($"{consumerKey}:QueueUpdated"),
                    c => c.WithGroup(group))
                .DeserializeKeyWithProto()
                .DeserializeValueWithProto()
                .HandleWith<QueueUpdatedHandler>())
            .AddConsumer(selector => selector
                .WithKey<SubjectCourseCreatedKey>()
                .WithValue<SubjectCourseCreatedValue>()
                .WithConfiguration(
                    configuration.GetSection($"{consumerKey}:SubjectCourseCreated"),
                    c => c.WithGroup(group))
                .DeserializeKeyWithProto()
                .DeserializeValueWithProto()
                .HandleWith<SubjectCourseCreatedHandler>())
            .AddConsumer(selector => selector
                .WithKey<SubjectCoursePointsUpdatedKey>()
                .WithValue<SubjectCoursePointsUpdatedValue>()
                .WithConfiguration(
                    configuration.GetSection($"{consumerKey}:SubjectCoursePointsUpdated"),
                    c => c.WithGroup(group))
                .DeserializeKeyWithProto()
                .DeserializeValueWithProto()
                .HandleWith<SubjectCoursePointsUpdatedHandler>())
            .AddConsumer(selector => selector
                .WithKey<StudentPointsUpdatedKey>()
                .WithValue<StudentPointsUpdatedValue>()
                .WithConfiguration(
                    configuration.GetSection($"{consumerKey}:StudentPointsUpdated"),
                    c => c.WithGroup(group))
                .DeserializeKeyWithProto()
                .DeserializeValueWithProto()
                .HandleWith<StudentPointsUpdatedHandler>()));

        return collection;
    }
}