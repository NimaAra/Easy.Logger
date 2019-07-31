namespace Easy.Logger.Extensions.Microsoft
{
    using System.IO;

    /// <summary>
    /// An abstraction for configuring <see cref="EasyLoggerProvider"/>.
    /// </summary>
    public sealed class EasyLoggerConfig : IEasyLoggerConfig
    {
        /// <summary>
        /// Gets the configuration file.
        /// </summary>
        public FileInfo ConfigFile { get; }

        /// <summary>
        /// Creates an instance of the <see cref="EasyLoggerConfig"/>.
        /// </summary>
        /// <param name="file">The configuration file.</param>
        public EasyLoggerConfig(FileInfo file) => ConfigFile = file;
    }
}