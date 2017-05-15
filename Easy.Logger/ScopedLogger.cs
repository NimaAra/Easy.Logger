namespace Easy.Logger
{
    using System;

    /// <summary>
    /// Provides scoped logging.
    /// </summary>
    public struct ScopedLogger : IDisposable
    {
        private readonly ILogger _logger;
        private readonly string _scopeName;
        private readonly LogLevel _level;

        internal ScopedLogger(ILogger logger, string scopeName, LogLevel level)
        {
            _scopeName = scopeName;
            _logger = logger;
            _level = level;

            Log("[-BEGIN---> " + _scopeName + "]");
        }

        /// <summary>
        /// Closes the scope.
        /// </summary>
        public void Dispose() => Log("[--END----> " + _scopeName + "]");

        private void Log(string message)
        {
            switch (_level)
            {
                case LogLevel.Debug:
                    _logger.Debug(message);
                    break;
                case LogLevel.Info:
                    _logger.Info(message);
                    break;
                case LogLevel.Warn:
                    _logger.Warn(message);
                    break;
                case LogLevel.Error:
                    _logger.Error(message);
                    break;
                case LogLevel.Fatal:
                    _logger.Fatal(message);
                    break;
            }
        }
    }
}