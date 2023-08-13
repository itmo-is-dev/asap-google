using Itmo.Dev.Asap.Google.Application.Extensions;
using Itmo.Dev.Asap.Google.Application.Handlers.Extensions;
using Itmo.Dev.Asap.Google.DataAccess.Extensions;
using Itmo.Dev.Asap.Google.Presentation.Grpc.Extensions;
using Itmo.Dev.Asap.Google.Presentation.Kafka.Extensions;
using Itmo.Dev.Asap.Google.Spreadsheets.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddUserSecrets<Program>();

builder.Services
    .AddGoogleApplication()
    .AddGoogleApplicationHandlers()
    .AddGoogleInfrastructureIntegration()
    .AddDataAccess()
    .AddGrpcPresentation()
    .AddKafkaPresentation(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();

WebApplication app = builder.Build();

await using (AsyncServiceScope scope = app.Services.CreateAsyncScope())
{
    await scope.UseDataAccessAsync(default);
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors(o => o.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
app.UseRouting();

app.UseRpcPresentation();

await app.RunAsync();