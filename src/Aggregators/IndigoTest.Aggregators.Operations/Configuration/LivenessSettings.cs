namespace IndigoTest.Aggregators.Operations.Configuration;

public sealed class LivenessSettings
{
    public required TimeSpan IdleTimeout { get; init; }
}