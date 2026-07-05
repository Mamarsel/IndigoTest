using Microsoft.Extensions.Logging;

namespace IndigoTest.Aggregators.Operations.Sources;

internal sealed partial class SourceListener
{
    private static partial class Log
    {
        [LoggerMessage(
            EventId = 1,
            Level = LogLevel.Information,
            SkipEnabledCheck = true,
            Message = "Connecting to source '{Source}' at '{Url}'.")]
        public static partial void LogConnectingToSource(ILogger logger, string source, string url);

        [LoggerMessage(
            EventId = 2,
            Level = LogLevel.Warning,
            SkipEnabledCheck = true,
            Message = "Source '{Source}' has timed out.")]
        public static partial void LogSourceTimedOut(ILogger logger, Exception exception, string source);

        [LoggerMessage(
            EventId = 3,
            Level = LogLevel.Warning,
            SkipEnabledCheck = true,
            Message = "Source '{Source}' connection failed.")]
        public static partial void LogSourceConnectionFailed(ILogger logger, Exception exception, string source);

        [LoggerMessage(
            EventId = 4,
            Level = LogLevel.Error,
            SkipEnabledCheck = true,
            Message = "Source '{Source}' listener failed.")]
        public static partial void LogSourceListenerFailed(ILogger logger, Exception exception, string source);

        [LoggerMessage(
            EventId = 5,
            Level = LogLevel.Information,
            SkipEnabledCheck = true,
            Message = "Reconnecting source '{Source}' in '{Delay}'.")]
        public static partial void LogReconnectingSource(ILogger logger, string source, TimeSpan delay);

        [LoggerMessage(
            EventId = 6,
            Level = LogLevel.Information,
            SkipEnabledCheck = true,
            Message = "Source '{Source}' closed connection.")]
        public static partial void LogSourceClosedConnection(ILogger logger, string source);
    }
}
