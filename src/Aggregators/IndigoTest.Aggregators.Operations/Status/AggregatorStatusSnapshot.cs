namespace IndigoTest.Aggregators.Operations.Status;

public sealed class AggregatorStatusSnapshot
{
    public required long PendingTicks { get; init; }

    public required long ReceivedTicks { get; init; }

    public required long ProcessedTicks { get; init; }

    public required long DuplicateTicks { get; init; }

    public required long WrittenTicks { get; init; }

    public required long DroppedTicks { get; init; }
}
