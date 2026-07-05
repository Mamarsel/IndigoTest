using IndigoTest.Aggregators.Operations.Configuration;
using Microsoft.Extensions.Options;

namespace IndigoTest.Aggregators.Operations.Sources;

internal sealed class SourceLivenessMonitorFactory(IOptions<AggregatorOptions> options) : ISourceLivenessMonitorFactory
{
    private readonly AggregatorOptions options = options.Value;

    public ISourceLivenessMonitor Create(SourceDefinition source)
    {
        return new SourceLivenessMonitor(options);
    }
}
