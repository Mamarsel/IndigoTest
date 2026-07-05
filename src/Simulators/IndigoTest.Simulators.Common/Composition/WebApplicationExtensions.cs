using IndigoTest.Simulators.Common.Configuration;
using IndigoTest.Simulators.Common.Faults;
using IndigoTest.Simulators.Common.Status;
using IndigoTest.Simulators.Common.Streaming;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace IndigoTest.Simulators.Common.Composition;

public static class WebApplicationExtensions
{
    public static WebApplication MapSimulatorEndpoints(this WebApplication app)
    {
        var options = app.Services.GetRequiredService<IOptions<SimulatorOptions>>().Value;

        app.MapGet("/api/status", (SimulatorStatusProvider statusProvider) =>
            Results.Ok((object?)statusProvider.GetSnapshot()));

        app.MapPost("/api/state", (SimulatorState state, SimulatorStateStore stateStore) =>
        {
            var errors = SimulatorStateValidation.Validate(state);
            return errors.Count > 0
                ? Results.ValidationProblem(ToDictionary(errors))
                : Results.Ok(stateStore.Update(state));
        });

        app.Map(options.WebSocketPath, async context =>
        {
            if (!context.WebSockets.IsWebSocketRequest)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;

                await context.Response.WriteAsync("WebSocket connection is expected.", context.RequestAborted);

                return;
            }

            var stateStore = context.RequestServices.GetRequiredService<SimulatorStateStore>();
            if (stateStore.TryGetUnavailableUntilUtc(out var unavailableUntil))
            {
                context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                context.Response.Headers.RetryAfter = Math.Max(
                    1,
                    (int)Math.Ceiling((unavailableUntil!.Value - DateTimeOffset.UtcNow).TotalSeconds)).ToString();

                await context.Response.WriteAsync("Simulator is temporarily unavailable.", context.RequestAborted);

                return;
            }

            var socket = await context.WebSockets.AcceptWebSocketAsync();
            var handler = context.RequestServices.GetRequiredService<WebSocketConnectionHandler>();

            await handler.HandleAsync(socket, context.RequestAborted);
        });

        return app;
    }

    private static Dictionary<string, string[]> ToDictionary(IReadOnlyList<string> errors)
    {
        return errors
            .Select((message, index) => new KeyValuePair<string, string[]>($"errors[{index}]", [message]))
            .ToDictionary();
    }
}
