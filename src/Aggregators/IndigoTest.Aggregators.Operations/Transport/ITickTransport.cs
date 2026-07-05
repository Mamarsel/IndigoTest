using IndigoTest.Aggregators.Operations.Ticks;

namespace IndigoTest.Aggregators.Operations.Transport;

public interface ITickTransport
{
    IAsyncEnumerable<NormalizedTick> ReadAllAsync(CancellationToken ct);

    ValueTask WriteAsync(NormalizedTick tick, CancellationToken ct);

    void Complete();
}
