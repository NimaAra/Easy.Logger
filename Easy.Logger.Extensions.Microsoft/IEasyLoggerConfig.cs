namespace Easy.Logger.Extensions.Microsoft
{
    using System.IO;

    /// <summary>
    /// Specifies the contract for an implementation of <see cref="IEasyLoggerConfig"/>.
    /// </summary>
    public interface IEasyLoggerConfig
    {
        /// <summary>
        /// Gets the configuration file.
        /// </summary>
        FileInfo ConfigFile { get; }
    }
}