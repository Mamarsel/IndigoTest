using IndigoTest.Simulators.Common.Faults;

namespace IndigoTest.Simulators.Common.Status;

public sealed class SimulatorStatusSnapshot
{
    public required string SourceName { get; init; }

    public required string WebSocketPath { get; init; }

    public required int ActiveConnections { get; init; }

    public required long TotalGeneratedTicks { get; init; }

    public required long TotalSentMessages { get; init; }

    public required long TotalDuplicateMessages { get; init; }

    public string? LastSymbol { get; init; }

    public DateTimeOffset? LastSentAt { get; init; }

    public required bool IsUnavailable { get; init; }

    public DateTimeOffset? UnavailableUntil { get; init; }

    public required SimulatorState State { get; init; }
}
