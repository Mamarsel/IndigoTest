using IndigoTest.Aggregators.Operations.Ticks;

namespace IndigoTest.Aggregators.Operations.Processing;

public interface ITickProcessor
{
    ValueTask ProcessAsync(IAsyncEnumerable<NormalizedTick> ticks, CancellationToken ct);
}
