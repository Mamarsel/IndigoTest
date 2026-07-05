namespace IndigoTest.Aggregators.Operations.Configuration;

public sealed class DeduplicationSettings
{
    public required TimeSpan Window { get; init; }
}