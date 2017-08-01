namespace Easy.Logger.Interfaces
{
    using System;
    using System.IO;

    /// <summary>
    /// The <see cref="ILogService"/> specifying the methods relating
    /// to configuring and obtaining an instance of <see cref="IEasyLogger"/>.
    /// </summary>
    public interface ILogService : IDisposable
    {
        /// <summary>
        /// Gets the configuration file used to configure the <see cref="ILogService"/>.
        /// </summary>
        FileInfo Configuration { get; }
        
        /// <summary>
        /// Configures the logging service by using the specified configuration file.
        /// </summary>
        void Configure(FileInfo configFile);

        /// <summary>
        /// Obtains an <see cref="IEasyLogger"/> for the given <paramref name="loggerName"/>.
        /// </summary>
        /// <param name="loggerName">The name for which an <see cref="IEasyLogger"/> should be returned</param>
        /// <returns>The <see cref="IEasyLogger"/></returns>
        IEasyLogger GetLogger(string loggerName);

        /// <summary>
        /// Obtains an <see cref="IEasyLogger"/> for the given <paramref name="loggerType"/>.
        /// </summary>
        /// <param name="loggerType">The <see cref="Type"/> for which an <see cref="IEasyLogger"/> should be returned</param>
        /// <returns>The <see cref="IEasyLogger"/></returns>
        IEasyLogger GetLogger(Type loggerType);

        /// <summary>
        /// Obtains an <see cref="IEasyLogger"/> for the given <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type for which an <see cref="IEasyLogger"/> should be returned</typeparam>
        /// <returns>The <see cref="IEasyLogger"/></returns>
        IEasyLogger GetLogger<T>();
    }
}