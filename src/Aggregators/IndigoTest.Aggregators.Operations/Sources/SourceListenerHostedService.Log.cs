using Microsoft.Extensions.Logging;

namespace IndigoTest.Aggregators.Operations.Sources;

internal sealed partial class SourceListenerHostedService
{
    private static partial class Log
    {
        [LoggerMessage(EventId = 1,
            Level = LogLevel.Warning,
            SkipEnabledCheck = true,
            Message = "No aggregator sources are configured.")]
        public static partial void LogNoAggregatorSourcesConfigured(ILogger logger);
    }
}
