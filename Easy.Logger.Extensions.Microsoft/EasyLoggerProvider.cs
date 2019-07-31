namespace Easy.Logger.Extensions.Microsoft
{
    using System.Diagnostics.CodeAnalysis;
    using global::Microsoft.Extensions.Logging;

    /// <summary>
    /// An implementation of the <see cref="ILoggerProvider"/>.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public sealed class EasyLoggerProvider : ILoggerProvider
    {
        /// <summary>
        /// Creates an instance of the <see cref="EasyLoggerProvider"/>.
        /// <remarks>A valid <c>log4net</c> configuration file must be present in the working directory.</remarks>
        /// </summary>
        public EasyLoggerProvider() { }

        /// <summary>
        /// Creates an instance of the <see cref="EasyLoggerProvider"/>.
        /// </summary>
        /// <param name="config">A valid <c>log4net</c> configuration file.</param>
        public EasyLoggerProvider(IEasyLoggerConfig config) => 
            Log4NetService.Instance.Configure(config.ConfigFile);

        /// <summary>
        /// Creates an <see cref="ILogger"/> with the given <paramref name="categoryName"/>.
        /// </summary>
        public ILogger CreateLogger(string categoryName) =>
            new EasyLogger(Log4NetService.Instance.GetLogger(categoryName));

        /// <summary>
        /// Disposes the instance flushing any pending log entries.
        /// </summary>
        public void Dispose() => Log4NetService.Instance.Dispose();
    }
}