using IndigoTest.Aggregators.Operations.Ticks;

namespace IndigoTest.Aggregators.Operations.Status;

internal sealed class AggregatorStatusTracker : IAggregatorStatus
{
    private const int MaxRecentTicks = 50;

    private readonly object sync = new();

    private readonly List<RecentTickSnapshot> recentTicks = [];

    private long pendingTicks;

    private long receivedTicks;

    private long processedTicks;

    private long duplicateTicks;

    private long writtenTicks;

    private long droppedTicks;

    public void MarkWritten(IReadOnlyList<NormalizedTick> ticks)
    {
        Interlocked.Add(ref writtenTicks, ticks.Count);

        lock (sync)
        {
            foreach (var tick in ticks)
            {
                recentTicks.Insert(0, new RecentTickSnapshot
                {
                    Source = tick.Source,
                    Symbol = tick.Symbol,
                    Price = tick.Price,
                    Volume = tick.Volume,
                    Timestamp = tick.Timestamp,
                    ReceivedAt = tick.ReceivedAt,
                });
            }

            if (recentTicks.Count > MaxRecentTicks)
            {
                recentTicks.RemoveRange(MaxRecentTicks, recentTicks.Count - MaxRecentTicks);
            }
        }
    }

    public AggregatorStatusSnapshot GetStatus()
    {
        return new AggregatorStatusSnapshot
        {
            PendingTicks = Interlocked.Read(ref pendingTicks),
            ReceivedTicks = Interlocked.Read(ref receivedTicks),
            ProcessedTicks = Interlocked.Read(ref processedTicks),
            DuplicateTicks = Interlocked.Read(ref duplicateTicks),
            WrittenTicks = Interlocked.Read(ref writtenTicks),
            DroppedTicks = Interlocked.Read(ref droppedTicks),
        };
    }

    public IReadOnlyList<RecentTickSnapshot> GetRecentTicks(int take)
    {
        lock (sync)
        {
            return recentTicks
                .Take(take)
                .ToArray();
        }
    }

    public void MarkDropped(int count)
    {
        Interlocked.Add(ref droppedTicks, count);
    }

    public void MarkQueued()
    {
        Interlocked.Increment(ref pendingTicks);
        Interlocked.Increment(ref receivedTicks);
    }

    public void MarkDequeued()
    {
        Interlocked.Decrement(ref pendingTicks);
    }

    public void MarkProcessed()
    {
        Interlocked.Increment(ref processedTicks);
    }

    public void MarkDuplicate()
    {
        Interlocked.Increment(ref duplicateTicks);
    }
}
