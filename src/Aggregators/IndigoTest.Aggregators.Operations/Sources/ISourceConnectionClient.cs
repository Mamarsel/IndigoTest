namespace IndigoTest.Aggregators.Operations.Sources;

public interface ISourceConnectionClient
{
    Task ConnectAsync(CancellationToken ct);

    ValueTask<ReceivedSourceMessage?> ReceiveAsync(CancellationToken ct);

    ValueTask CloseAsync(CancellationToken ct);
}
