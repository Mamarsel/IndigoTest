using IndigoTest.Simulators.Common.Faults;

namespace IndigoTest.Aggregators.Web;

public sealed class SimulatorDashboardSnapshot
{
    public required string Source { get; init; }

    public required string Kind { get; init; }

    public required string Url { get; init; }

    public required bool IsAvailable { get; init; }

    public string? Error { get; init; }

    public int? ActiveConnections { get; init; }

    public long? TotalGeneratedTicks { get; init; }

    public long? TotalSentMessages { get; init; }

    public long? TotalDuplicateMessages { get; init; }

    public string? LastSymbol { get; init; }

    public DateTimeOffset? LastSentAt { get; init; }

    public bool IsUnavailable { get; init; }

    public DateTimeOffset? UnavailableUntil { get; init; }

    public SimulatorState? State { get; init; }
}
