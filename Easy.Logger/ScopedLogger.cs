namespace Easy.Logger
{
    using System;
    using Easy.Logger.Interfaces;
    
    /// <summary>
    /// Provides scoped logging.
    /// </summary>
    public struct ScopedLogger : IDisposable
    {
        private const string OpenLeft = "[/ ";
        private const string OpenRight = @" \]";
        private const string CloseLeft = @"[\ ";
        private const string CloseRight = " /]";

        private readonly IEasyLogger _logger;
        private readonly string _scopeName;
        private readonly EasyLogLevel _level;

        internal ScopedLogger(IEasyLogger logger, string scopeName, EasyLogLevel level)
        {
            _scopeName = scopeName;
            _logger = logger;
            _level = level;

            Log(string.Concat(OpenLeft, _scopeName, OpenRight));
        }

        /// <summary>
        /// Closes the scope.
        /// </summary>
        public void Dispose() => Log(string.Concat(CloseLeft, _scopeName, CloseRight));

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