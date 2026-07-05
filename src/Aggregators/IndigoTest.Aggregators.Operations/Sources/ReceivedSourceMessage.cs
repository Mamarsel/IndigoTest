namespace IndigoTest.Aggregators.Operations.Sources;

public sealed class ReceivedSourceMessage
{
    public required string Payload { get; init; }

    public required DateTimeOffset ReceivedAt { get; init; }
}
