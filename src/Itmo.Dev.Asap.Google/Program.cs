#pragma warning disable CA1506

using Itmo.Dev.Asap.Google.Application.Extensions;
using Itmo.Dev.Asap.Google.Application.Handlers.Extensions;
using Itmo.Dev.Asap.Google.DataAccess.Extensions;
using Itmo.Dev.Asap.Google.Integrations.Github.Extensions;
using Itmo.Dev.Asap.Google.Presentation.Grpc.Extensions;
using Itmo.Dev.Asap.Google.Presentation.Kafka.Extensions;
using Itmo.Dev.Asap.Google.Spreadsheets.Extensions;
using Itmo.Dev.Platform.Logging.Extensions;
using Itmo.Dev.Platform.YandexCloud.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddUserSecrets<Program>();

await builder.AddYandexCloudConfigurationAsync();

builder.Services
    .AddGoogleApplication()
    .AddGoogleApplicationHandlers()
    .AddGoogleInfrastructureIntegration()
    .AddGithubIntegration()
    .AddDataAccess()
    .AddGrpcPresentation()
    .AddKafkaPresentation(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();

builder.AddPlatformSentry();
builder.Host.AddPlatformSerilog(builder.Configuration);

WebApplication app = builder.Build();

await using (AsyncServiceScope scope = app.Services.CreateAsyncScope())
{
    await scope.UseDataAccessAsync(default);
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors(o => o.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
app.UseRouting();
app.UsePlatformSentryTracing(builder.Configuration);

app.UseRpcPresentation();

await app.RunAsync();