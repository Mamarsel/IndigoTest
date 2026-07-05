using Microsoft.Extensions.Logging;

namespace IndigoTest.Aggregators.Operations.Processing;

internal sealed partial class TickProcessor
{
    private static partial class Log
    {
        [LoggerMessage(
            EventId = 1,
            Level = LogLevel.Warning,
            SkipEnabledCheck = true,
            Message = "Retrying batch write. Attempt '{Attempt}'. Batch size: '{BatchSize}'. Delay: '{Delay}'.")]
        public static partial void LogRetryingBatchWrite(
            ILogger logger,
            Exception exception,
            int attempt,
            int batchSize,
            TimeSpan delay);
    }
}
