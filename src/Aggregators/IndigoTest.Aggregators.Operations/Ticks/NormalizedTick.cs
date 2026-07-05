namespace IndigoTest.Aggregators.Operations.Ticks;

public sealed class NormalizedTick
{
    public required string Symbol { get; init; }

    public required decimal Price { get; init; }

    public required decimal Volume { get; init; }

    public required DateTimeOffset Timestamp { get; init; }

    public required string Source { get; init; }

    public required DateTimeOffset ReceivedAt { get; init; }
}
