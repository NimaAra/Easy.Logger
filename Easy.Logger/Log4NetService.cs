namespace Easy.Logger
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using Easy.Logger.Interfaces;
    using log4net;
    using log4net.Config;
    using log4net.Repository;

    /// <summary>
    /// An implementation of the <see cref="ILogService"/> based on <c>log4net</c>.
    /// </summary>
    public sealed class Log4NetService : ILogService
    {
        private readonly ILoggerRepository _repository;

        private static readonly Lazy<Log4NetService> Lazy = 
            new Lazy<Log4NetService>(() => new Log4NetService(), true);

        /// <summary>
        /// Returns a single instance of the <see cref="Log4NetService"/>
        /// </summary>
        public static Log4NetService Instance => Lazy.Value;

        /// <summary>
        /// Creates and configures an instance of the <see cref="Log4NetService"/> by looking for a 
        /// default <c>log4net.config</c> file in the executing directory and monitoring it for changes.
        /// </summary>
        /// <exception cref="FileNotFoundException">
        /// Thrown when a valid <c>log4net.config</c> file is not found.
        /// </exception>
        private Log4NetService()
        {
            string log4NetConfigDir;
            Assembly assembly;
#if NET_STANDARD
            log4NetConfigDir = AppContext.BaseDirectory;
            assembly = GetType().GetTypeInfo().Assembly;
#else
            log4NetConfigDir = AppDomain.CurrentDomain.BaseDirectory;
            assembly = GetType().Assembly;
#endif
            _repository = LogManager.GetRepository(assembly);
            
            var defaultConfigFile = new FileInfo(Path.Combine(log4NetConfigDir, "log4net.config"));
            if (!defaultConfigFile.Exists) { return; }
            
            ConfigureImpl(_repository, defaultConfigFile);
        }
        
        /// <summary>
        /// Gets the configuration file used to configure the <see cref="Log4NetService"/>.
        /// </summary>
        public FileInfo Configuration { get; private set; }

        /// <summary>
        /// Provides an override to configure the <see cref="Log4NetService"/> with a valid <c>log4net</c>
        /// config file represented as <paramref name="configFile"/> and monitor any changes to it.
        /// </summary> 
        /// <param name="configFile">Points to a valid log4net config file.</param>
        /// <exception cref="ArgumentException">Thrown when <c>log4net</c> config file is null.</exception>
        /// <exception cref="FileNotFoundException">Thrown when a valid <c>log4net.config</c> file is not found.</exception>
        /// <remarks>
        /// If this method is not used, the <see cref="Log4NetService"/> will be configured by looking for a 
        /// default <c>log4net.config</c> file in the executing directory.
        /// </remarks>
        public void Configure(FileInfo configFile)
        {
            if (configFile is null)
            {
                throw new ArgumentException(nameof(configFile) + " cannot be null", nameof(configFile));
            }

            if (!configFile.Exists)
            {
                throw new FileNotFoundException(
                    "Could not find a valid log4net configuration file", 
                    configFile.FullName);
            }

            ConfigureImpl(_repository, configFile);
        }

        /// <summary>
        /// Obtains an <see cref="IEasyLogger"/> for the given <paramref name="loggerName"/>.
        /// </summary>
        /// <param name="loggerName">The name for which an <see cref="IEasyLogger"/> should be returned</param>
        /// <exception cref="InvalidOperationException">Thrown if <see cref="Log4NetService"/> is not configured with a valid configuration file.</exception>
        /// <returns>The <see cref="IEasyLogger"/>An instance of the logger.</returns>
        [DebuggerStepThrough]
        public IEasyLogger GetLogger(string loggerName)
        {
            EnsureConfigured();
            return new Log4NetLogger(LogManager.GetLogger(_repository.Name, loggerName));
        }

        /// <summary>
        /// Obtains an <see cref="IEasyLogger"/> for the given <paramref name="loggerType"/>.
        /// </summary>
        /// <param name="loggerType">The <see cref="Type"/> for which an <see cref="IEasyLogger"/> should be returned</param>
        /// <exception cref="InvalidOperationException">Thrown if <see cref="Log4NetService"/> is not configured with a valid configuration file.</exception>
        /// <returns>The <see cref="IEasyLogger"/>An instance of the logger.</returns>
        [DebuggerStepThrough]
        public IEasyLogger GetLogger(Type loggerType)
        {
            EnsureConfigured();
            return new Log4NetLogger(LogManager.GetLogger(loggerType));
        }

        /// <summary>
        /// Obtains an <see cref="IEasyLogger"/> for the given <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type for which an <see cref="IEasyLogger"/> should be returned</typeparam>
        /// <exception cref="InvalidOperationException">Thrown if <see cref="Log4NetService"/> is not configured with a valid configuration file.</exception>
        /// <returns>The <see cref="IEasyLogger"/>An instance of the logger.</returns>
        [DebuggerStepThrough]
        public IEasyLogger GetLogger<T>()
        {
            EnsureConfigured();
            return new Log4NetLogger<T>();
        } 

        /// <summary>
        /// Disposes the <see cref="Log4NetService"/>
        /// </summary>
        [DebuggerStepThrough]
        public void Dispose() => LogManager.Shutdown();

        private void ConfigureImpl(ILoggerRepository repository, FileInfo configFile)
        {
            log4net.Util.SystemInfo.NullText = string.Empty;
            
            XmlConfigurator.ConfigureAndWatch(repository, configFile);
            Configuration = configFile;
        }

        private void EnsureConfigured()
        {
            if (Configuration != null) { return; }

            throw new InvalidOperationException(
                GetType().Name + " needs to be configured with a valid configuration file.");
        }
    }
}