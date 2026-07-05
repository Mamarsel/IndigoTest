using System.Net.WebSockets;
using IndigoTest.Aggregators.Operations.Transport;
using Microsoft.Extensions.Logging;

namespace IndigoTest.Aggregators.Operations.Sources;

internal sealed partial class SourceListener(
    ISourceConnectionClientFactory clients,
    ISourceMessageParserResolver parsers,
    ISourceLivenessMonitorFactory livenessMonitors,
    ISourceReconnectPolicy reconnectPolicy,
    ITickTransport transport,
    ILogger<SourceListener> logger)
    : ISourceListener
{
    private readonly ISourceConnectionClientFactory clients = clients;

    private readonly ISourceMessageParserResolver parsers = parsers;

    private readonly ISourceLivenessMonitorFactory livenessMonitors = livenessMonitors;

    private readonly ISourceReconnectPolicy reconnectPolicy = reconnectPolicy;

    private readonly ITickTransport transport = transport;

    private readonly ILogger<SourceListener> logger = logger;

    public async Task RunAsync(SourceDefinition source, CancellationToken ct)
    {
        var parser = parsers.Resolve(source);
        var reconnectAttempt = 0;

        while (!ct.IsCancellationRequested)
        {
            var client = clients.Create(source);
            var liveness = livenessMonitors.Create(source);

            try
            {
                Log.LogConnectingToSource(logger, source.Source, source.Url);

                await client.ConnectAsync(ct);

                reconnectAttempt = 0;
                liveness.Reset();

                await ReadMessagesAsync(source, client, parser, liveness, ct);
            }
            catch (OperationCanceledException) when (ct.IsCancellationRequested)
            {
                break;
            }
            catch (TimeoutException exception)
            {
                Log.LogSourceTimedOut(logger, exception, source.Source);
            }
            catch (WebSocketException exception)
            {
                Log.LogSourceConnectionFailed(logger, exception, source.Source);
            }
            catch (Exception exception)
            {
                Log.LogSourceListenerFailed(logger, exception, source.Source);
            }
            finally
            {
                await client.CloseAsync(CancellationToken.None);
            }

            reconnectAttempt++;

            var delay = reconnectPolicy.GetDelay(reconnectAttempt);

            Log.LogReconnectingSource(logger, source.Source, delay);

            await Task.Delay(delay, ct);
        }
    }

    private async Task ReadMessagesAsync(
        SourceDefinition source,
        ISourceConnectionClient client,
        ISourceMessageParser parser,
        ISourceLivenessMonitor liveness,
        CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            var message = await ReceiveMessageAsync(source, client, liveness, ct);

            if (message is null)
            {
                Log.LogSourceClosedConnection(logger, source.Source);

                return;
            }

            liveness.MarkMessage(message.ReceivedAt);

            var tick = parser.Parse(message, source);

            if (tick == null)
            {
                continue;
            }

            await transport.WriteAsync(tick, ct);
        }
    }

    private static async Task<ReceivedSourceMessage?> ReceiveMessageAsync(
        SourceDefinition source,
        ISourceConnectionClient client,
        ISourceLivenessMonitor liveness,
        CancellationToken ct)
    {
        var receiveTask = client.ReceiveAsync(ct).AsTask();
        var delay = TimeSpan.FromMilliseconds(250);

        while (!receiveTask.IsCompleted)
        {
            if (liveness.HasTimedOut(DateTimeOffset.UtcNow))
            {
                throw new TimeoutException($"Source '{source.Source}' has stopped sending messages.");
            }

            await Task.Delay(delay, ct);
        }

        return await receiveTask;
    }
}
