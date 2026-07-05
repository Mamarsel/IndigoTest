namespace IndigoTest.Aggregator.Domain;

public sealed class MarketTick
{
    public Guid ID { get; init; } = Guid.NewGuid();

    public required string Symbol { get; init; }

    public required decimal Price { get; init; }

    public required decimal Volume { get; init; }

    public required DateTimeOffset Timestamp { get; init; }

    public required string Source { get; init; }

    public DateTimeOffset ReceivedAt { get; init; } = DateTimeOffset.UtcNow;
}
