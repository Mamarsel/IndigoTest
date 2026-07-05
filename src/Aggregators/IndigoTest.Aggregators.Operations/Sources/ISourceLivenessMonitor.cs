namespace IndigoTest.Aggregators.Operations.Sources;

public interface ISourceLivenessMonitor
{
    void MarkMessage(DateTimeOffset receivedAt);

    bool HasTimedOut(DateTimeOffset now);

    void Reset();
}
