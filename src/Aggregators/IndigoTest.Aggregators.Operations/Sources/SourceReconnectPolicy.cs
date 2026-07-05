using IndigoTest.Aggregators.Operations.Configuration;
using Microsoft.Extensions.Options;

namespace IndigoTest.Aggregators.Operations.Sources;

internal sealed class SourceReconnectPolicy(IOptions<AggregatorOptions> options) : ISourceReconnectPolicy
{
    private readonly ReconnectSettings options = options.Value.Reconnect;

    public TimeSpan GetDelay(int attempt)
    {
        if (attempt <= 0)
        {
            return options.InitialDelay;
        }

        var delay = TimeSpan.FromMilliseconds(options.InitialDelay.TotalMilliseconds * Math.Pow(2, attempt - 1));

        return delay > options.MaxDelay
            ? options.MaxDelay
            : delay;
    }
}
