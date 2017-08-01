namespace Easy.Logger
{
    using System;
    using Easy.Logger.Interfaces;
    
    /// <summary>
    /// Provides scoped logging.
    /// </summary>
    public struct ScopedLogger : IDisposable
    {
        private readonly IEasyLogger _logger;
        private readonly string _scopeName;
        private readonly EasyLogLevel _level;

        internal ScopedLogger(IEasyLogger logger, string scopeName, EasyLogLevel level)
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
                case EasyLogLevel.Debug:
                    _logger.Debug(message);
                    break;
                case EasyLogLevel.Info:
                    _logger.Info(message);
                    break;
                case EasyLogLevel.Warn:
                    _logger.Warn(message);
                    break;
                case EasyLogLevel.Error:
                    _logger.Error(message);
                    break;
                case EasyLogLevel.Fatal:
                    _logger.Fatal(message);
                    break;
            }
        }
    }
}