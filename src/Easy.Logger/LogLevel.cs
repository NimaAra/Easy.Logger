namespace Easy.Logger
{
    /// <summary>
    /// Determines the level as which a log entry should be logged.
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// The trace level.
        /// </summary>
        Trace = 0,

        /// <summary>
        /// The debug level.
        /// </summary>
        Debug,

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