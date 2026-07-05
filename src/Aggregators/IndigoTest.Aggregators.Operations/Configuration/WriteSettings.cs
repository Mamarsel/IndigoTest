namespace IndigoTest.Aggregators.Operations.Configuration;

public sealed class WriteSettings
{
    public required int BatchSize { get; init; }

    public required TimeSpan RetryDelay { get; init; }
}