using IndigoTest.Aggregators.Operations.Ticks;

namespace IndigoTest.Aggregators.Operations.Processing;

internal readonly record struct TickDeduplicationKey(string Source,
    string Symbol,
    DateTimeOffset Timestamp,
    decimal Price,
    decimal Volume)
{
    public static TickDeduplicationKey Create(NormalizedTick tick)
    {
        return new TickDeduplicationKey(tick.Source,
            tick.Symbol,
            tick.Timestamp,
            tick.Price,
            tick.Volume);
    }
}
