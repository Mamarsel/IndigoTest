using IndigoTest.Aggregators.Operations.Sources;

namespace IndigoTest.Aggregators.Operations.Configuration;

public sealed class AggregatorOptions
{
    public const string Section = "Aggregator";

    public required SourceDefinition[] Sources { get; init; }

    public required LivenessSettings Liveness { get; init; }

    public required ReconnectSettings Reconnect { get; init; }

    public required TransportSettings Transport { get; init; }

    public required DeduplicationSettings Deduplication { get; init; }

    public required WriteSettings Write { get; init; }
}