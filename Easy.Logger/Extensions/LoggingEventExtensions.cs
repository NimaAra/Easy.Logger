namespace Easy.Logger.Extensions
{
    using log4net.Core;

    internal static class LoggingEventExtensions
    {
        internal static string GetLoggerName(this LoggingEvent logEvent)
        {
            var loggerName = logEvent.LoggerName;

            if (string.IsNullOrWhiteSpace(loggerName) || loggerName.Length == 1) { return loggerName; }

            int startIdx;
            var lastCharIdx = loggerName.Length - 1;
            const char Dot = '.';
            startIdx = loggerName[lastCharIdx] == Dot 
                ? loggerName.IndexOf(Dot) : loggerName.LastIndexOf(Dot);
            
            if (startIdx < 0 || startIdx == lastCharIdx) { return loggerName; }

            return loggerName.Substring(startIdx + 1, lastCharIdx - startIdx);
        }
    }
}