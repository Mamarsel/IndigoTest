using System.Net.WebSockets;
using System.Text;
using IndigoTest.Simulators.Common.Configuration;
using IndigoTest.Simulators.Common.Contracts;
using IndigoTest.Simulators.Common.Faults;
using IndigoTest.Simulators.Common.MarketData;
using IndigoTest.Simulators.Common.Status;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace IndigoTest.Simulators.Common.Streaming;

public sealed class WebSocketConnectionHandler(
    ISimulatorMessageFactory messageFactory,
    TickGenerator tickGenerator,
    IOptions<SimulatorOptions> options,
    SimulatorStateStore stateStore,
    SimulatorStatusProvider statusProvider,
    ILogger<WebSocketConnectionHandler> logger)
{
    private readonly ISimulatorMessageFactory messageFactory = messageFactory;

    private readonly TickGenerator tickGenerator = tickGenerator;

    private readonly SimulatorOptions options = options.Value;

    private readonly SimulatorStateStore stateStore = stateStore;

    private readonly SimulatorStatusProvider statusProvider = statusProvider;

    private readonly ILogger<WebSocketConnectionHandler> logger = logger;

    public async Task HandleAsync(WebSocket socket, CancellationToken ct)
    {
        statusProvider.RegisterConnectionOpened();

        try
        {
            while (!ct.IsCancellationRequested && socket.State == WebSocketState.Open)
            {
                if (stateStore.TryGetUnavailableUntilUtc(out _))
                {
                    await CloseAsUnavailableAsync(socket, ct);

                    return;
                }

                for (var index = 0; index < options.TicksPerIteration; index++)
                {
                    if (socket.State != WebSocketState.Open)
                    {
                        return;
                    }

                    if (stateStore.TryGetUnavailableUntilUtc(out _))
                    {
                        await CloseAsUnavailableAsync(socket, ct);

                        return;
                    }

                    var tick = tickGenerator.Generate();
                    statusProvider.RegisterTickGenerated();

                    await SendAsync(socket, tick, false, ct);

                    if (stateStore.ShouldStartOutageAfterMessage())
                    {
                        await CloseAsUnavailableAsync(socket, ct);

                        return;
                    }

                    if (stateStore.ShouldDisconnectAfterMessage())
                    {
                        await CloseAsync(socket, ct);

                        return;
                    }

                    var silenceDelay = stateStore.GetSilenceDelayAfterMessage();
                    if (silenceDelay is not null)
                    {
                        await Task.Delay(silenceDelay.Value, ct);
                    }

                    if (!stateStore.ShouldSendDuplicate())
                    {
                        continue;
                    }

                    await SendAsync(socket, tick, true, ct);

                    if (stateStore.ShouldStartOutageAfterMessage())
                    {
                        await CloseAsUnavailableAsync(socket, ct);

                        return;
                    }

                    if (stateStore.ShouldDisconnectAfterMessage())
                    {
                        await CloseAsync(socket, ct);

                        return;
                    }

                    silenceDelay = stateStore.GetSilenceDelayAfterMessage();
                    if (silenceDelay is not null)
                    {
                        await Task.Delay(silenceDelay.Value, ct);
                    }
                }

                await Task.Delay(options.TickIntervalMs, ct);
            }
        }
        catch (OperationCanceledException) when (ct.IsCancellationRequested)
        {
        }
        catch (WebSocketException exception)
        {
            logger.LogWarning(exception, "WebSocket session for {SourceName} was interrupted.", options.SourceName);
        }
        finally
        {
            statusProvider.RegisterConnectionClosed();

            socket.Dispose();
        }
    }

    private async Task SendAsync(WebSocket socket, SimulatedTick tick, bool isDuplicate, CancellationToken ct)
    {
        var payload = messageFactory.Create(tick);
        var buffer = Encoding.UTF8.GetBytes(payload);

        await socket.SendAsync(buffer, WebSocketMessageType.Text, true, ct);

        statusProvider.RegisterMessageSent(tick, isDuplicate);
    }

    private static async Task CloseAsync(WebSocket socket, CancellationToken ct)
    {
        if (socket.State is WebSocketState.Open or WebSocketState.CloseReceived)
        {
            await socket.CloseAsync(
                WebSocketCloseStatus.NormalClosure,
                "Disconnect requested by simulator state.",
                ct);
        }
    }

    private static async Task CloseAsUnavailableAsync(WebSocket socket, CancellationToken ct)
    {
        if (socket.State is WebSocketState.Open or WebSocketState.CloseReceived)
        {
            await socket.CloseAsync(
                WebSocketCloseStatus.EndpointUnavailable,
                "Simulator is temporarily unavailable.",
                ct);
        }
    }
}
