using IndigoTest.Aggregators.Operations.Configuration;
using IndigoTest.Aggregators.Operations.Transport;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace IndigoTest.Aggregators.Operations.Sources;

internal sealed partial class SourceListenerHostedService(
    IOptions<AggregatorOptions> options,
    ISourceListener listener,
    ITickTransport transport,
    ILogger<SourceListenerHostedService> logger)
    : BackgroundService
{
    private readonly AggregatorOptions options = options.Value;

    private readonly ISourceListener listener = listener;

    private readonly ITickTransport transport = transport;

    private readonly ILogger<SourceListenerHostedService> logger = logger;

    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        if (options.Sources.Length == 0)
        {
            Log.LogNoAggregatorSourcesConfigured(logger);

            transport.Complete();

            return;
        }

        var tasks = options.Sources
            .Select(source => listener.RunAsync(source, ct));

        try
        {
            await Task.WhenAll(tasks);
        }
        finally
        {
            transport.Complete();
        }
    }

    public override async Task StopAsync(CancellationToken ct)
    {
        await base.StopAsync(ct);

        transport.Complete();
    }
}
