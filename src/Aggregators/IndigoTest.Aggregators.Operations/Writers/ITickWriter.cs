using IndigoTest.Aggregators.Operations.Ticks;

namespace IndigoTest.Aggregators.Operations.Writers;

public interface ITickWriter
{
    Task WriteAsync(TickWriteBatch batch, CancellationToken ct);
}
