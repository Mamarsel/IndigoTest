using IndigoTest.Aggregators.Operations.Configuration;

namespace IndigoTest.Aggregators.Operations.Sources;

internal sealed class SourceLivenessMonitor(AggregatorOptions options) : ISourceLivenessMonitor
{
    private readonly LivenessSettings options = options.Liveness;

    private DateTimeOffset lastMessageAt = DateTimeOffset.UtcNow;

    public void MarkMessage(DateTimeOffset receivedAt)
    {
        lastMessageAt = receivedAt;
    }

    public bool HasTimedOut(DateTimeOffset now)
    {
        return now - lastMessageAt > options.IdleTimeout;
    }

    public void Reset()
    {
        lastMessageAt = DateTimeOffset.UtcNow;
    }
}
