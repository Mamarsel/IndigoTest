namespace IndigoTest.Aggregators.Operations.Sources;

public sealed class SourceDefinition
{
    public required string Source { get; init; }

    public required string Kind { get; init; }

    public required string Url { get; init; }
}
