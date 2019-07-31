namespace Easy.Logger.Extensions.Microsoft
{
    using System;
    using Easy.Logger.Interfaces;
    using global::Microsoft.Extensions.Logging;

    /// <summary>
    /// An implementation of the <see cref="ILogger"/> based on
    /// <see href="https://github.com/NimaAra/Easy.Logger"/>.
    /// </summary>
    public sealed class EasyLogger : ILogger
    {
        private readonly IEasyLogger _logger;

        /// <summary>
        /// Creates an instance of the <see cref="EasyLogger"/>.
        /// </summary>
        // ReSharper disable once UnusedMember.Global
        public EasyLogger() { }

        /// <summary>
        /// Creates an instance of the <see cref="EasyLogger"/>.
        /// </summary>
        /// <param name="easyLogger">An instance of the <see cref="IEasyLogger"/>.</param>
        public EasyLogger(IEasyLogger easyLogger) => _logger = easyLogger;
        
        /// <summary>
        /// Gets the flag indicating whether the given <paramref name="logLevel"/> is enabled.
        /// </summary>
        public bool IsEnabled(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Trace:
                    return _logger.IsTraceEnabled;
                case LogLevel.Debug:
                    return _logger.IsDebugEnabled;
                case LogLevel.Information:
                    return _logger.IsInfoEnabled;
                case LogLevel.Warning:
                    return _logger.IsWarnEnabled;
                case LogLevel.Error:
                    return _logger.IsErrorEnabled;
                case LogLevel.Critical:
                    return _logger.IsFatalEnabled;
                default:
                    throw new ArgumentOutOfRangeException(nameof(logLevel), logLevel, null);
            }
        }

        /// <summary>
        /// Returns an <see cref="T:System.IDisposable" /> which allows the caller to specify a scope as
        /// <paramref name="state" /> which will then be rendered as part of the message.
        /// </summary>
        /// <param name="state">The scope identifier.</param>
        public IDisposable BeginScope<TState>(TState state) => _logger.GetScopedLogger(state.ToString());

        /// <summary>
        /// Does the logging.
        /// </summary>
        public void Log<TState>(
            LogLevel logLevel, 
            EventId eventId, 
            TState state, 
            Exception exception, 
            Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel)) { return; }

            if (formatter is null) { throw new ArgumentNullException(nameof(formatter)); }

            var message = formatter(state, exception);

            if (message is null) { return; }

            switch (logLevel)
            {
                case LogLevel.Trace:
                    _logger.Trace(message, exception);
                    break;
                case LogLevel.Debug:
                    _logger.Debug(message, exception);
                    break;
                case LogLevel.Information:
                    _logger.Info(message, exception);
                    break;
                case LogLevel.Warning:
                    _logger.Warn(message, exception);
                    break;
                case LogLevel.Error:
                    _logger.Error(message, exception);
                    break;
                case LogLevel.Critical:
                    _logger.Fatal(message, exception);
                    break;
                default:
                    _logger.WarnFormat("Invalid {0}: {1} | {2}", nameof(logLevel), logLevel, message);
                    break;
            }
        }
    }
}