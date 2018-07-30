namespace Easy.Logger.Extensions.Microsoft
{
    using System.IO;
    using global::Microsoft.Extensions.Logging;

    /// <summary>
    /// An implementation of the <see cref="ILoggerProvider"/>.
    /// </summary>
    public sealed class EasyLoggerProvider : ILoggerProvider
    {
        /// <summary>
        /// Creates an instance of the <see cref="EasyLoggerProvider"/>.
        /// <remarks>The current implementation is based on <c>log4net</c>.</remarks>
        /// </summary>
        /// <param name="config">A valid configuration file.</param>
        public EasyLoggerProvider(FileInfo config = null)
        {
            if (!(config is null))
            {
                Log4NetService.Instance.Configure(config);
            }
        }

        /// <summary>
        /// Creates an <see cref="ILogger"/> with the given <paramref name="categoryName"/>.
        /// </summary>
        public ILogger CreateLogger(string categoryName)
            => new EasyLogger(Log4NetService.Instance.GetLogger(categoryName));

        /// <summary>
        /// Disposes the instance.
        /// </summary>
        public void Dispose() { }
    }
}