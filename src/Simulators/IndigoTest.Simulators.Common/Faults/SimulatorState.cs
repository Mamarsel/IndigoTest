namespace IndigoTest.Simulators.Common.Faults;

public sealed record SimulatorState
{
    public bool DuplicatesEnabled { get; init; }

    public double DuplicateChance { get; init; }

    public bool SilenceEnabled { get; init; }

    public int? SilenceDurationMs { get; init; }

    public int? SilenceAfterMessages { get; init; }

    public bool OutageEnabled { get; init; }

    public int? OutageDurationMs { get; init; }

    public int? OutageAfterMessages { get; init; }

    public bool CancelOutageRequested { get; init; }

    public DisconnectMode DisconnectMode { get; init; }

    public int? DisconnectAfterMessages { get; init; }
}
