namespace Easy.Logger.Extensions.Microsoft
{
    using System.IO;
    using global::Microsoft.Extensions.DependencyInjection;
    using global::Microsoft.Extensions.Logging;

    /// <summary>
    /// A set of methods to extend the behavior of <see cref="ILoggingBuilder"/>.
    /// </summary>
    public static class LoggingBuilderExtensions
    {
        /// <summary>
        /// Adds <see cref="EasyLoggerProvider"/> to <paramref name="builder"/>.
        /// </summary>
        public static void AddEasyLogger(this ILoggingBuilder builder) =>
            builder.Services.AddSingleton<ILoggerProvider, EasyLoggerProvider>();

        /// <summary>
        /// Adds <see cref="EasyLoggerProvider"/> to <paramref name="builder"/>.
        /// </summary>
        /// <param name="builder">An instance of the <see cref="ILoggingBuilder"/>.</param>
        /// <param name="log4netConfig">A valid configuration file.</param>
        public static void AddEasyLogger(this ILoggingBuilder builder, FileInfo log4netConfig)
        {
            builder.Services.AddSingleton<IEasyLoggerConfig>(new EasyLoggerConfig(log4netConfig));
            builder.AddEasyLogger();
        }
    }
}