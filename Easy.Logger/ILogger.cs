namespace Easy.Logger
{
    using System;

    /// <summary>
    /// The <see cref="ILogger"/> interface specifying 
    /// available methods for different levels of logging.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Logs a <c>Debug</c> level message object.
        /// </summary>
        /// <param name="message">The message object to be logged.</param>
        void Debug(object message);

        /// <summary>
        /// Logs a <c>Debug</c> level message object including the stack trace of 
        /// the <see cref="T:System.Exception"/> passed as a parameter.
        /// </summary>
        /// <param name="message">The message object to be logged.</param>
        /// <param name="exception">The exception to be logged, including its stack trace.</param>
        void Debug(object message, Exception exception);

        /// <summary>
        /// Logs a <c>Debug</c> level formatted message string with the given <paramref name="arg"/>.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="arg">The object to format</param>
        void DebugFormat(string format, object arg);

        /// <summary>
        /// Logs a <c>Debug</c> level formatted message string with the given arguments.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="arg1">The first object to format</param>
        /// <param name="arg2">The second object to format</param>
        void DebugFormat(string format, object arg1, object arg2);

        /// <summary>
        /// Logs a <c>Debug</c> level formatted message string with the given arguments.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="arg1">The first object to format</param>
        /// <param name="arg2">The second object to format</param>
        /// <param name="arg3">The third object to format</param>
        void DebugFormat(string format, object arg1, object arg2, object arg3);

        /// <summary>
        /// Logs a <c>Debug</c> level formatted message string with the given <paramref name="args"/>.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="args">An Object array containing zero or more objects to format</param>
        void DebugFormat(string format, params object[] args);

        /// <summary>
        /// Logs a <c>Debug</c> level formatted message string with the 
        /// given <paramref name="args"/> and a given <paramref name="provider"/>.
        /// </summary>
        /// <param name="provider">An <see cref= "T:System.IFormatProvider"/> that supplies culture-specific formatting information</param>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="args">An Object array containing zero or more objects to format</param>
        void DebugFormat(IFormatProvider provider, string format, params object[] args);

        /// <summary>
        /// Logs a <c>Info</c> level message object.
        /// </summary>
        /// <param name="message">The message object to be logged.</param>
        void Info(object message);

        /// <summary>
        /// Logs a <c>Info</c> level message object including the stack trace of 
        /// the <see cref="T:System.Exception"/> passed as a parameter.
        /// </summary>
        /// <param name="message">The message object to be logged.</param>
        /// <param name="exception">The exception to be logged, including its stack trace.</param>
        void Info(object message, Exception exception);

        /// <summary>
        /// Logs a <c>Info</c> level formatted message string with the given <paramref name="arg"/>.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="arg">The object to format</param>
        void InfoFormat(string format, object arg);

        /// <summary>
        /// Logs a <c>Info</c> level formatted message string with the given arguments.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="arg1">The first object to format</param>
        /// <param name="arg2">The second object to format</param>
        void InfoFormat(string format, object arg1, object arg2);

        /// <summary>
        /// Logs a <c>Info</c> level formatted message string with the given arguments.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="arg1">The first object to format</param>
        /// <param name="arg2">The second object to format</param>
        /// <param name="arg3">The third object to format</param>
        void InfoFormat(string format, object arg1, object arg2, object arg3);

        /// <summary>
        /// Logs a <c>Info</c> level formatted message string with the given <paramref name="args"/>.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="args">An Object array containing zero or more objects to format</param>
        void InfoFormat(string format, params object[] args);

        /// <summary>
        /// Logs a <c>Info</c> level formatted message string with the 
        /// given <paramref name="args"/> and a given <paramref name="provider"/>.
        /// </summary>
        /// <param name="provider">An <see cref= "T:System.IFormatProvider"/> that supplies culture-specific formatting information</param>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="args">An Object array containing zero or more objects to format</param>
        void InfoFormat(IFormatProvider provider, string format, params object[] args);

        /// <summary>
        /// Logs a <c>Warn</c> level message object.
        /// </summary>
        /// <param name="message">The message object to be logged.</param>
        void Warn(object message);

        /// <summary>
        /// Logs a <c>Warn</c> level message object including the stack trace of 
        /// the <see cref="T:System.Exception"/> passed as a parameter.
        /// </summary>
        /// <param name="message">The message object to be logged.</param>
        /// <param name="exception">The exception to be logged, including its stack trace.</param>
        void Warn(object message, Exception exception);

        /// <summary>
        /// Logs a <c>Warn</c> level formatted message string with the given <paramref name="arg"/>.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="arg">The object to format</param>
        void WarnFormat(string format, object arg);

        /// <summary>
        /// Logs a <c>Warn</c> level formatted message string with the given arguments.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="arg1">The first object to format</param>
        /// <param name="arg2">The second object to format</param>
        void WarnFormat(string format, object arg1, object arg2);

        /// <summary>
        /// Logs a <c>Warn</c> level formatted message string with the given arguments.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="arg1">The first object to format</param>
        /// <param name="arg2">The second object to format</param>
        /// <param name="arg3">The third object to format</param>
        void WarnFormat(string format, object arg1, object arg2, object arg3);

        /// <summary>
        /// Logs a <c>Warn</c> level formatted message string with the given <paramref name="args"/>.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="args">An Object array containing zero or more objects to format</param>
        void WarnFormat(string format, params object[] args);

        /// <summary>
        /// Logs a <c>Warn</c> level formatted message string with the 
        /// given <paramref name="args"/> and a given <paramref name="provider"/>.
        /// </summary>
        /// <param name="provider">An <see cref= "T:System.IFormatProvider"/> that supplies culture-specific formatting information</param>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="args">An Object array containing zero or more objects to format</param>
        void WarnFormat(IFormatProvider provider, string format, params object[] args);

        /// <summary>
        /// Logs a <c>Error</c> level message object.
        /// </summary>
        /// <param name="message">The message object to be logged.</param>
        void Error(object message);

        /// <summary>
        /// Logs a <c>Error</c> level message object including the stack trace of 
        /// the <see cref="T:System.Exception"/> passed as a parameter.
        /// </summary>
        /// <param name="message">The message object to be logged.</param>
        /// <param name="exception">The exception to be logged, including its stack trace.</param>
        void Error(object message, Exception exception);

        /// <summary>
        /// Logs a <c>Error</c> level formatted message string with the given <paramref name="arg"/>.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="arg">The object to format</param>
        void ErrorFormat(string format, object arg);

        /// <summary>
        /// Logs a <c>Error</c> level formatted message string with the given arguments.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="arg1">The first object to format</param>
        /// <param name="arg2">The second object to format</param>
        void ErrorFormat(string format, object arg1, object arg2);

        /// <summary>
        /// Logs a <c>Error</c> level formatted message string with the given arguments.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="arg1">The first object to format</param>
        /// <param name="arg2">The second object to format</param>
        /// <param name="arg3">The third object to format</param>
        void ErrorFormat(string format, object arg1, object arg2, object arg3);

        /// <summary>
        /// Logs a <c>Error</c> level formatted message string with the given <paramref name="args"/>.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="args">An Object array containing zero or more objects to format</param>
        void ErrorFormat(string format, params object[] args);

        /// <summary>
        /// Logs a <c>Error</c> level formatted message string with the 
        /// given <paramref name="args"/> and a given <paramref name="provider"/>.
        /// </summary>
        /// <param name="provider">An <see cref= "T:System.IFormatProvider"/> that supplies culture-specific formatting information</param>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="args">An Object array containing zero or more objects to format</param>
        void ErrorFormat(IFormatProvider provider, string format, params object[] args);

        /// <summary>
        /// Logs a <c>Fatal</c> level message object.
        /// </summary>
        /// <param name="message">The message object to be logged.</param>
        void Fatal(object message);

        /// <summary>
        /// Logs a <c>Fatal</c> level message object including the stack trace of 
        /// the <see cref="T:System.Exception"/> passed as a parameter.
        /// </summary>
        /// <param name="message">The message object to be logged.</param>
        /// <param name="exception">The exception to be logged, including its stack trace.</param>
        void Fatal(object message, Exception exception);

        /// <summary>
        /// Logs a <c>Fatal</c> level formatted message string with the given <paramref name="arg"/>.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="arg">The object to format</param>
        void FatalFormat(string format, object arg);

        /// <summary>
        /// Logs a <c>Fatal</c> level formatted message string with the given arguments.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="arg1">The first object to format</param>
        /// <param name="arg2">The second object to format</param>
        void FatalFormat(string format, object arg1, object arg2);

        /// <summary>
        /// Logs a <c>Fatal</c> level formatted message string with the given arguments.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="arg1">The first object to format</param>
        /// <param name="arg2">The second object to format</param>
        /// <param name="arg3">The third object to format</param>
        void FatalFormat(string format, object arg1, object arg2, object arg3);

        /// <summary>
        /// Logs a <c>Fatal</c> level formatted message string with the given <paramref name="args"/>.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="args">An Object array containing zero or more objects to format</param>
        void FatalFormat(string format, params object[] args);

        /// <summary>
        /// Logs a <c>Fatal</c> level formatted message string with the 
        /// given <paramref name="args"/> and a given <paramref name="provider"/>.
        /// </summary>
        /// <param name="provider">An <see cref= "T:System.IFormatProvider"/> that supplies culture-specific formatting information</param>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="args">An Object array containing zero or more objects to format</param>
        void FatalFormat(IFormatProvider provider, string format, params object[] args);
    }
}