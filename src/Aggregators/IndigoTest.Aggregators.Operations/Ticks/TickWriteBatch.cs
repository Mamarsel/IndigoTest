namespace IndigoTest.Aggregators.Operations.Ticks;

public sealed class TickWriteBatch
{
    public required List<NormalizedTick> Items { get; init; }
}
