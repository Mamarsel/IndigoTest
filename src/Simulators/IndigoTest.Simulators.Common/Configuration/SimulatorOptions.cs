namespace IndigoTest.Simulators.Common.Configuration;

public sealed class SimulatorOptions
{
    public const string Section = "Simulator";

    public required string SourceName { get; init; }

    public required string WebSocketPath { get; init; }

    public required int TickIntervalMs { get; init; }

    public required int TicksPerIteration { get; init; }

    public required string[] Symbols { get; init; }

    public required decimal PriceStep { get; init; }

    public required decimal MinVolume { get; init; }

    public required decimal MaxVolume { get; init; }
}
