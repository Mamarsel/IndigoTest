using System.Threading.Channels;
using System.Runtime.CompilerServices;
using IndigoTest.Aggregators.Operations.Configuration;
using IndigoTest.Aggregators.Operations.Status;
using IndigoTest.Aggregators.Operations.Ticks;
using Microsoft.Extensions.Options;

namespace IndigoTest.Aggregators.Operations.Transport;

internal sealed class TickTransport(
    IOptions<AggregatorOptions> options,
    IAggregatorStatus status)
    : ITickTransport
{
    private readonly IAggregatorStatus status = status;

    private readonly Channel<NormalizedTick> channel = Channel.CreateBounded<NormalizedTick>(new BoundedChannelOptions(options.Value.Transport.Capacity)
    {
        FullMode = BoundedChannelFullMode.Wait,
        SingleReader = true,
        SingleWriter = false,
    });

    public async IAsyncEnumerable<NormalizedTick> ReadAllAsync([EnumeratorCancellation] CancellationToken ct)
    {
        await foreach (var tick in channel.Reader.ReadAllAsync(ct))
        {
            status.MarkDequeued();

            yield return tick;
        }
    }

    public async ValueTask WriteAsync(NormalizedTick tick, CancellationToken ct)
    {
        await channel.Writer.WriteAsync(tick, ct);

        status.MarkQueued();
    }

    public void Complete()
    {
        channel.Writer.TryComplete();
    }
}
