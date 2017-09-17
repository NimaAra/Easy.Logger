// ReSharper disable once CheckNamespace
namespace Easy.Logger
{
    using Easy.Logger.Interfaces;

    /// <summary>
    /// Set of extension methods for <see cref="IEasyLogger"/>.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public static class ILoggerExtensions
    {
        /// <summary>
        /// Returns a <see cref="ScopedLogger"/> which provides ability to mark the 
        /// begin and end of a scope defined by <paramref name="name"></paramref>.
        /// </summary>
        /// <param name="logger">The logger</param>
        /// <param name="name">The name of the scope</param>
        /// <param name="level">The level at which the scope should be logged</param>
        /// <returns>The scoped logger</returns>
        public static ScopedLogger GetScopedLogger(this IEasyLogger logger, string name, EasyLogLevel level) => 
            new ScopedLogger(logger, name, level);
    }
}