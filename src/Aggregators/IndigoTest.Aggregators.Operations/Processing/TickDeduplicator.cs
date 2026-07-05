using System.Collections.Concurrent;
using IndigoTest.Aggregators.Operations.Configuration;
using IndigoTest.Aggregators.Operations.Ticks;
using Microsoft.Extensions.Options;

namespace IndigoTest.Aggregators.Operations.Processing;

internal sealed class TickDeduplicator(IOptions<AggregatorOptions> options) : ITickDeduplicator
{
    private const int CleanupThreshold = 1_024;

    private readonly ConcurrentDictionary<TickDeduplicationKey, DateTimeOffset> items = new();

    private readonly DeduplicationSettings options = options.Value.Deduplication;

    private int processedSinceCleanup;

    public bool IsDuplicate(NormalizedTick tick)
    {
        var now = DateTimeOffset.UtcNow;
        var key = TickDeduplicationKey.Create(tick);

        TryCleanupExpired(now);

        while (true)
        {
            if (!items.TryGetValue(key, out var existingAt))
            {
                if (items.TryAdd(key, now))
                {
                    return false;
                }

                continue;
            }

            if (now - existingAt <= options.Window)
            {
                return true;
            }

            if (items.TryUpdate(key, now, existingAt))
            {
                return false;
            }
        }
    }

    private void TryCleanupExpired(DateTimeOffset now)
    {
        var processed = Interlocked.Increment(ref processedSinceCleanup);

        if (processed < CleanupThreshold)
        {
            return;
        }

        var snapshot = Interlocked.Exchange(ref processedSinceCleanup, 0);
        if (snapshot < CleanupThreshold)
        {
            return;
        }

        foreach (var item in items)
        {
            if (now - item.Value <= options.Window)
            {
                continue;
            }

            items.TryRemove(new KeyValuePair<TickDeduplicationKey, DateTimeOffset>(item.Key, item.Value));
        }
    }
}
