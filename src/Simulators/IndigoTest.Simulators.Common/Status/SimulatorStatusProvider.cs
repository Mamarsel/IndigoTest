using IndigoTest.Simulators.Common.Configuration;
using IndigoTest.Simulators.Common.Faults;
using IndigoTest.Simulators.Common.MarketData;
using Microsoft.Extensions.Options;

namespace IndigoTest.Simulators.Common.Status;

public sealed class SimulatorStatusProvider(
    IOptions<SimulatorOptions> options,
    SimulatorStateStore stateStore)
{
    private readonly object sync = new();

    private readonly SimulatorOptions options = options.Value;

    private long activeConnections;

    private long totalGeneratedTicks;

    private long totalSentMessages;

    private long totalDuplicateMessages;

    private string? lastSymbol;

    private DateTimeOffset? lastSentAt;

    public void RegisterConnectionOpened()
    {
        Interlocked.Increment(ref activeConnections);
    }

    public void RegisterConnectionClosed()
    {
        Interlocked.Decrement(ref activeConnections);
    }

    public void RegisterTickGenerated()
    {
        Interlocked.Increment(ref totalGeneratedTicks);
    }

    public void RegisterMessageSent(SimulatedTick tick, bool isDuplicate)
    {
        Interlocked.Increment(ref totalSentMessages);

        if (isDuplicate)
        {
            Interlocked.Increment(ref totalDuplicateMessages);
        }

        lock (sync)
        {
            lastSymbol = tick.Symbol;
            lastSentAt = tick.Timestamp;
        }
    }

    public SimulatorStatusSnapshot GetSnapshot()
    {
        string? currentSymbol;
        DateTimeOffset? currentLastSentAt;
        var isUnavailable = stateStore.TryGetUnavailableUntilUtc(out var unavailableUntil);

        lock (sync)
        {
            currentSymbol = lastSymbol;
            currentLastSentAt = lastSentAt;
        }

        return new SimulatorStatusSnapshot
        {
            SourceName = options.SourceName,
            WebSocketPath = options.WebSocketPath,
            ActiveConnections = (int)Interlocked.Read(ref activeConnections),
            TotalGeneratedTicks = Interlocked.Read(ref totalGeneratedTicks),
            TotalSentMessages = Interlocked.Read(ref totalSentMessages),
            TotalDuplicateMessages = Interlocked.Read(ref totalDuplicateMessages),
            LastSymbol = currentSymbol,
            LastSentAt = currentLastSentAt,
            IsUnavailable = isUnavailable,
            UnavailableUntil = unavailableUntil,
            State = stateStore.Get(),
        };
    }
}
