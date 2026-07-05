using IndigoTest.Aggregators.Operations.Ticks;

namespace IndigoTest.Aggregators.Operations.Status;

public interface IAggregatorStatus
{
    AggregatorStatusSnapshot GetStatus();

    IReadOnlyList<RecentTickSnapshot> GetRecentTicks(int take);

    void MarkWritten(IReadOnlyList<NormalizedTick> ticks);

    void MarkDropped(int count);

    void MarkQueued();

    void MarkDequeued();

    void MarkProcessed();

    void MarkDuplicate();
}
