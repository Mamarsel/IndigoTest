using IndigoTest.Aggregators.Operations.Configuration;
using IndigoTest.Aggregators.Operations.Status;
using IndigoTest.Aggregators.Operations.Ticks;
using IndigoTest.Aggregators.Operations.Writers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace IndigoTest.Aggregators.Operations.Processing;

internal sealed partial class TickProcessor(
    ITickDeduplicator deduplicator,
    IOptions<AggregatorOptions> options,
    ITickWriter writer,
    IAggregatorStatus status,
    ILogger<TickProcessor> logger)
    : ITickProcessor
{
    private readonly ITickDeduplicator deduplicator = deduplicator;

    private readonly IAggregatorStatus status = status;

    private readonly ITickWriter writer = writer;

    private readonly ILogger<TickProcessor> logger = logger;

    private readonly AggregatorOptions options = options.Value;

    public async ValueTask ProcessAsync(
        IAsyncEnumerable<NormalizedTick> ticks,
        CancellationToken ct)
    {
        var items = new List<NormalizedTick>(options.Write.BatchSize);

        await foreach (var tick in ticks.WithCancellation(ct))
        {
            status.MarkProcessed();

            if (deduplicator.IsDuplicate(tick))
            {
                status.MarkDuplicate();

                continue;
            }

            items.Add(tick);

            if (items.Count >= options.Write.BatchSize)
            {
                await FlushAsync(items, ct);
            }
        }

        if (items.Count > 0)
        {
            await FlushAsync(items, ct);
        }
    }

    private async ValueTask FlushAsync(
        List<NormalizedTick> items,
        CancellationToken ct)
    {
        var batch = new TickWriteBatch
        {
            Items = [.. items],
        };

        items.Clear();

        await WriteBatchAsync(batch, ct);
    }

    private async ValueTask WriteBatchAsync(
        TickWriteBatch batch,
        CancellationToken ct)
    {
        var attempt = 0;

        while (true)
        {
            try
            {
                await writer.WriteAsync(batch, ct);

                status.MarkWritten(batch.Items);

                return;
            }
            catch (OperationCanceledException) when (ct.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception exception)
            {
                attempt++;

                Log.LogRetryingBatchWrite(
                    logger,
                    exception,
                    attempt,
                    batch.Items.Count,
                    options.Write.RetryDelay);

                await Task.Delay(options.Write.RetryDelay, ct);
            }
        }
    }
}
