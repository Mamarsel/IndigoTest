using IndigoTest.Aggregators.Operations.Configuration;
using IndigoTest.Aggregators.Operations.Composition;
using IndigoTest.Aggregators.Operations.Status;
using IndigoTest.Aggregators.Persistence;
using IndigoTest.Aggregators.Web;
using IndigoTest.Simulators.Common.Faults;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddHttpClient();
builder.Services.AddTransient<SimulatorControlService>();

builder.Services.AddAggregatorOperations(builder.Configuration);
builder.Services.AddAggregatorPersistence(builder.Configuration);

var app = builder.Build();

await using (var scope = app.Services.CreateAsyncScope())
{
    var db = scope.ServiceProvider.GetRequiredService<DbContext>();

    await db.Database.MigrateAsync();
}

app.MapGet(
    "/api/status",
    (IAggregatorStatus status) => Results.Ok(status.GetStatus()));

app.MapGet(
    "/api/dashboard",
    async (IAggregatorStatus status, IOptions<AggregatorOptions> options, SimulatorControlService simulators, CancellationToken ct) => Results.Ok(new
    {
        Status = status.GetStatus(),
        RecentTicks = status.GetRecentTicks(25),
        Sources = options.Value.Sources,
        Simulators = await simulators.GetSnapshotsAsync(ct),
    }));

app.MapGet(
    "/api/simulators",
    async (SimulatorControlService simulators, CancellationToken ct) => Results.Ok(await simulators.GetSnapshotsAsync(ct)));

app.MapPost(
    "/api/simulators/{source}/state",
    async (string source, SimulatorState state, SimulatorControlService simulators, CancellationToken ct) =>
    {
        var snapshot = await simulators.UpdateStateAsync(source, state, ct);

        return snapshot == null
            ? Results.NotFound()
            : Results.Ok(snapshot);
    });

app.MapGet(
    "/",
    () => Results.Content(DashboardPage.Html, "text/html; charset=utf-8"));

app.Run();
