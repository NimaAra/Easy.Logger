namespace Easy.Logger.Extensions.Microsoft
{
    using System.IO;
    using global::Microsoft.Extensions.Logging;

    /// <summary>
    /// A set of methods to extend the behavior of <see cref="ILoggerFactory"/>.
    /// </summary>
    public static class EasyLoggerFactoryExtensions
    {
        /// <summary>
        /// Adds the <see href="https://github.com/NimaAra/Easy.Logger"/> to <paramref name="factory"/>.
        /// </summary>
        public static ILoggerFactory AddEasyLogger(this ILoggerFactory factory)
            => AddEasyLogger(factory, null);

        /// <summary>
        /// Adds the <see href="https://github.com/NimaAra/Easy.Logger"/> to <paramref name="factory"/>.
        /// </summary>
        /// <param name="factory">An instance of the <see cref="ILoggerFactory"/>.</param>
        /// <param name="config">A valid configuration file.</param>
        public static ILoggerFactory AddEasyLogger(this ILoggerFactory factory, FileInfo config)
        {
            factory.AddProvider(new EasyLoggerProvider(config));
            return factory;
        }
    }
}