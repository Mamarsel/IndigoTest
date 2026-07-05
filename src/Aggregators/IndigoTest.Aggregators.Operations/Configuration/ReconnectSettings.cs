namespace IndigoTest.Aggregators.Operations.Configuration;

public sealed class ReconnectSettings
{
    public required TimeSpan InitialDelay { get; init; }

    public required TimeSpan MaxDelay { get; init; }
}