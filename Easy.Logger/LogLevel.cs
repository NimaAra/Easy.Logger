namespace Easy.Logger
{
    /// <summary>
    /// Determines the level as which a log entry should be logged.
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// The debug level.
        /// </summary>
        Debug = 0,

        /// <summary>
        /// The information level.
        /// </summary>
        Info,

        /// <summary>
        /// The warning level.
        /// </summary>
        Warn,

        /// <summary>
        /// The error level.
        /// </summary>
        Error,

        /// <summary>
        /// The fatal level.
        /// </summary>
        Fatal
    }
}