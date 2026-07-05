using System.Net.WebSockets;
using System.Text;

namespace IndigoTest.Aggregators.Operations.Sources;

internal sealed class WebSocketSourceConnectionClient(SourceDefinition source) : ISourceConnectionClient
{
    private readonly SourceDefinition source = source;

    private ClientWebSocket? socket;

    public async Task ConnectAsync(CancellationToken ct)
    {
        socket = new ClientWebSocket();

        await socket.ConnectAsync(new Uri(source.Url), ct);
    }

    public async ValueTask<ReceivedSourceMessage?> ReceiveAsync(CancellationToken ct)
    {
        if (socket is null)
        {
            throw new InvalidOperationException("Source connection is not initialized.");
        }

        var buffer = new byte[4 * 1024];
        using var payload = new MemoryStream();

        while (true)
        {
            var result = await socket.ReceiveAsync(buffer, ct);

            if (result.MessageType == WebSocketMessageType.Close)
            {
                return null;
            }

            payload.Write(buffer, 0, result.Count);

            if (result.EndOfMessage)
            {
                break;
            }
        }

        var text = Encoding.UTF8.GetString(payload.ToArray());

        return new ReceivedSourceMessage
        {
            Payload = text,
            ReceivedAt = DateTimeOffset.UtcNow,
        };
    }

    public async ValueTask CloseAsync(CancellationToken ct)
    {
        if (socket is null)
        {
            return;
        }

        if (socket.State is WebSocketState.Open or WebSocketState.CloseReceived)
        {
            await socket.CloseAsync(WebSocketCloseStatus.NormalClosure,
                "Source listener is closing connection.",
                ct);
        }

        socket.Dispose();
        socket = null;
    }
}
