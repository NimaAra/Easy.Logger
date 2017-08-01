namespace Easy.Logger.Interfaces
{
    using System;

    /// <summary>
    /// The <see cref="IEasyLogger"/> interface specifying 
    /// available methods for different levels of logging.
    /// </summary>
    public interface IEasyLogger
    {
        /// <summary>
        /// Gets the logger name.
        /// </summary>
        string Name { get; }
        
        /// <summary>
        /// Logs a <see cref="EasyLogLevel.Trace"/> level message object.
        /// </summary>
        /// <param name="message">The message object to be logged.</param>
        void Trace(object message);

        /// <summary>
        /// Logs a <see cref="EasyLogLevel.Trace"/> level message object including the stack trace of 
        /// the <see cref="T:System.Exception"/> passed as a parameter.
        /// </summary>
        /// <param name="message">The message object to be logged.</param>
        /// <param name="exception">The exception to be logged, including its stack trace.</param>
        void Trace(object message, Exception exception);

        /// <summary>
        /// Logs a <see cref="EasyLogLevel.Trace"/> level formatted message string with the given <paramref name="arg"/>.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="arg">The object to format</param>
        void TraceFormat(string format, object arg);

        /// <summary>
        /// Logs a <see cref="EasyLogLevel.Trace"/> level formatted message string with the given arguments.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="arg1">The first object to format</param>
        /// <param name="arg2">The second object to format</param>
        void TraceFormat(string format, object arg1, object arg2);

        /// <summary>
        /// Logs a <see cref="EasyLogLevel.Trace"/> level formatted message string with the given arguments.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="arg1">The first object to format</param>
        /// <param name="arg2">The second object to format</param>
        /// <param name="arg3">The third object to format</param>
        void TraceFormat(string format, object arg1, object arg2, object arg3);

        /// <summary>
        /// Logs a <see cref="EasyLogLevel.Trace"/> level formatted message string with the given <paramref name="args"/>.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="args">An Object array containing zero or more objects to format</param>
        void TraceFormat(string format, params object[] args);

        /// <summary>
        /// Logs a <see cref="EasyLogLevel.Trace"/> level formatted message string with the 
        /// given <paramref name="args"/> and a given <paramref name="provider"/>.
        /// </summary>
        /// <param name="provider">An <see cref= "T:System.IFormatProvider"/> that supplies culture-specific formatting information</param>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="args">An Object array containing zero or more objects to format</param>
        void TraceFormat(IFormatProvider provider, string format, params object[] args);

        /// <summary>
        /// Logs a <see cref="EasyLogLevel.Debug"/> level message object.
        /// </summary>
        /// <param name="message">The message object to be logged.</param>
        void Debug(object message);

        /// <summary>
        /// Logs a <see cref="EasyLogLevel.Debug"/> level message object including the stack trace of 
        /// the <see cref="T:System.Exception"/> passed as a parameter.
        /// </summary>
        /// <param name="message">The message object to be logged.</param>
        /// <param name="exception">The exception to be logged, including its stack trace.</param>
        void Debug(object message, Exception exception);

        /// <summary>
        /// Logs a <see cref="EasyLogLevel.Debug"/> level formatted message string with the given <paramref name="arg"/>.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="arg">The object to format</param>
        void DebugFormat(string format, object arg);

        /// <summary>
        /// Logs a <see cref="EasyLogLevel.Debug"/> level formatted message string with the given arguments.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="arg1">The first object to format</param>
        /// <param name="arg2">The second object to format</param>
        void DebugFormat(string format, object arg1, object arg2);

        /// <summary>
        /// Logs a <see cref="EasyLogLevel.Debug"/> level formatted message string with the given arguments.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="arg1">The first object to format</param>
        /// <param name="arg2">The second object to format</param>
        /// <param name="arg3">The third object to format</param>
        void DebugFormat(string format, object arg1, object arg2, object arg3);

        /// <summary>
        /// Logs a <see cref="EasyLogLevel.Debug"/> level formatted message string with the given <paramref name="args"/>.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="args">An Object array containing zero or more objects to format</param>
        void DebugFormat(string format, params object[] args);

        /// <summary>
        /// Logs a <see cref="EasyLogLevel.Debug"/> level formatted message string with the 
        /// given <paramref name="args"/> and a given <paramref name="provider"/>.
        /// </summary>
        /// <param name="provider">An <see cref= "T:System.IFormatProvider"/> that supplies culture-specific formatting information</param>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="args">An Object array containing zero or more objects to format</param>
        void DebugFormat(IFormatProvider provider, string format, params object[] args);

        /// <summary>
        /// Logs a <see cref="EasyLogLevel.Info"/> level message object.
        /// </summary>
        /// <param name="message">The message object to be logged.</param>
        void Info(object message);

        /// <summary>
        /// Logs a <see cref="EasyLogLevel.Info"/> level message object including the stack trace of 
        /// the <see cref="T:System.Exception"/> passed as a parameter.
        /// </summary>
        /// <param name="message">The message object to be logged.</param>
        /// <param name="exception">The exception to be logged, including its stack trace.</param>
        void Info(object message, Exception exception);

        /// <summary>
        /// Logs a <see cref="EasyLogLevel.Info"/> level formatted message string with the given <paramref name="arg"/>.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="arg">The object to format</param>
        void InfoFormat(string format, object arg);

        /// <summary>
        /// Logs a <see cref="EasyLogLevel.Info"/> level formatted message string with the given arguments.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="arg1">The first object to format</param>
        /// <param name="arg2">The second object to format</param>
        void InfoFormat(string format, object arg1, object arg2);

        /// <summary>
        /// Logs a <see cref="EasyLogLevel.Info"/> level formatted message string with the given arguments.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="arg1">The first object to format</param>
        /// <param name="arg2">The second object to format</param>
        /// <param name="arg3">The third object to format</param>
        void InfoFormat(string format, object arg1, object arg2, object arg3);

        /// <summary>
        /// Logs a <see cref="EasyLogLevel.Info"/> level formatted message string with the given <paramref name="args"/>.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="args">An Object array containing zero or more objects to format</param>
        void InfoFormat(string format, params object[] args);

        /// <summary>
        /// Logs a <see cref="EasyLogLevel.Info"/> level formatted message string with the 
        /// given <paramref name="args"/> and a given <paramref name="provider"/>.
        /// </summary>
        /// <param name="provider">An <see cref= "T:System.IFormatProvider"/> that supplies culture-specific formatting information</param>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="args">An Object array containing zero or more objects to format</param>
        void InfoFormat(IFormatProvider provider, string format, params object[] args);

        /// <summary>
        /// Logs a <see cref="EasyLogLevel.Warn"/> level message object.
        /// </summary>
        /// <param name="message">The message object to be logged.</param>
        void Warn(object message);

        /// <summary>
        /// Logs a <see cref="EasyLogLevel.Warn"/> level message object including the stack trace of 
        /// the <see cref="T:System.Exception"/> passed as a parameter.
        /// </summary>
        /// <param name="message">The message object to be logged.</param>
        /// <param name="exception">The exception to be logged, including its stack trace.</param>
        void Warn(object message, Exception exception);

        /// <summary>
        /// Logs a <see cref="EasyLogLevel.Warn"/> level formatted message string with the given <paramref name="arg"/>.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="arg">The object to format</param>
        void WarnFormat(string format, object arg);

        /// <summary>
        /// Logs a <see cref="EasyLogLevel.Warn"/> level formatted message string with the given arguments.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="arg1">The first object to format</param>
        /// <param name="arg2">The second object to format</param>
        void WarnFormat(string format, object arg1, object arg2);

        /// <summary>
        /// Logs a <see cref="EasyLogLevel.Warn"/> level formatted message string with the given arguments.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="arg1">The first object to format</param>
        /// <param name="arg2">The second object to format</param>
        /// <param name="arg3">The third object to format</param>
        void WarnFormat(string format, object arg1, object arg2, object arg3);

        /// <summary>
        /// Logs a <see cref="EasyLogLevel.Warn"/> level formatted message string with the given <paramref name="args"/>.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="args">An Object array containing zero or more objects to format</param>
        void WarnFormat(string format, params object[] args);

        /// <summary>
        /// Logs a <see cref="EasyLogLevel.Warn"/> level formatted message string with the 
        /// given <paramref name="args"/> and a given <paramref name="provider"/>.
        /// </summary>
        /// <param name="provider">An <see cref= "T:System.IFormatProvider"/> that supplies culture-specific formatting information</param>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="args">An Object array containing zero or more objects to format</param>
        void WarnFormat(IFormatProvider provider, string format, params object[] args);

        /// <summary>
        /// Logs a <see cref="EasyLogLevel.Error"/> level message object.
        /// </summary>
        /// <param name="message">The message object to be logged.</param>
        void Error(object message);

        /// <summary>
        /// Logs a <see cref="EasyLogLevel.Error"/> level message object including the stack trace of 
        /// the <see cref="T:System.Exception"/> passed as a parameter.
        /// </summary>
        /// <param name="message">The message object to be logged.</param>
        /// <param name="exception">The exception to be logged, including its stack trace.</param>
        void Error(object message, Exception exception);

        /// <summary>
        /// Logs a <see cref="EasyLogLevel.Error"/> level formatted message string with the given <paramref name="arg"/>.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="arg">The object to format</param>
        void ErrorFormat(string format, object arg);

        /// <summary>
        /// Logs a <see cref="EasyLogLevel.Error"/> level formatted message string with the given arguments.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="arg1">The first object to format</param>
        /// <param name="arg2">The second object to format</param>
        void ErrorFormat(string format, object arg1, object arg2);

        /// <summary>
        /// Logs a <see cref="EasyLogLevel.Error"/> level formatted message string with the given arguments.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="arg1">The first object to format</param>
        /// <param name="arg2">The second object to format</param>
        /// <param name="arg3">The third object to format</param>
        void ErrorFormat(string format, object arg1, object arg2, object arg3);

        /// <summary>
        /// Logs a <see cref="EasyLogLevel.Error"/> level formatted message string with the given <paramref name="args"/>.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="args">An Object array containing zero or more objects to format</param>
        void ErrorFormat(string format, params object[] args);

        /// <summary>
        /// Logs a <see cref="EasyLogLevel.Error"/> level formatted message string with the 
        /// given <paramref name="args"/> and a given <paramref name="provider"/>.
        /// </summary>
        /// <param name="provider">An <see cref= "T:System.IFormatProvider"/> that supplies culture-specific formatting information</param>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="args">An Object array containing zero or more objects to format</param>
        void ErrorFormat(IFormatProvider provider, string format, params object[] args);

        /// <summary>
        /// Logs a <see cref="EasyLogLevel.Fatal"/> level message object.
        /// </summary>
        /// <param name="message">The message object to be logged.</param>
        void Fatal(object message);

        /// <summary>
        /// Logs a <see cref="EasyLogLevel.Fatal"/> level message object including the stack trace of 
        /// the <see cref="T:System.Exception"/> passed as a parameter.
        /// </summary>
        /// <param name="message">The message object to be logged.</param>
        /// <param name="exception">The exception to be logged, including its stack trace.</param>
        void Fatal(object message, Exception exception);

        /// <summary>
        /// Logs a <see cref="EasyLogLevel.Fatal"/> level formatted message string with the given <paramref name="arg"/>.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="arg">The object to format</param>
        void FatalFormat(string format, object arg);

        /// <summary>
        /// Logs a <see cref="EasyLogLevel.Fatal"/> level formatted message string with the given arguments.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="arg1">The first object to format</param>
        /// <param name="arg2">The second object to format</param>
        void FatalFormat(string format, object arg1, object arg2);

        /// <summary>
        /// Logs a <see cref="EasyLogLevel.Fatal"/> level formatted message string with the given arguments.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="arg1">The first object to format</param>
        /// <param name="arg2">The second object to format</param>
        /// <param name="arg3">The third object to format</param>
        void FatalFormat(string format, object arg1, object arg2, object arg3);

        /// <summary>
        /// Logs a <see cref="EasyLogLevel.Fatal"/> level formatted message string with the given <paramref name="args"/>.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="args">An Object array containing zero or more objects to format</param>
        void FatalFormat(string format, params object[] args);

        /// <summary>
        /// Logs a <see cref="EasyLogLevel.Fatal"/> level formatted message string with the 
        /// given <paramref name="args"/> and a given <paramref name="provider"/>.
        /// </summary>
        /// <param name="provider">An <see cref= "T:System.IFormatProvider"/> that supplies culture-specific formatting information</param>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="args">An Object array containing zero or more objects to format</param>
        void FatalFormat(IFormatProvider provider, string format, params object[] args);

        /// <summary>
        /// Flag indicating whether the logger is enabled fur <see cref="EasyLogLevel.Trace"/> messages
        /// </summary>
        bool IsTraceEnabled { get; }

        /// <summary>
        /// Flag indicating whether the logger is enabled fur <see cref="EasyLogLevel.Debug"/> messages
        /// </summary>
        bool IsDebugEnabled { get; }

        /// <summary>
        /// Flag indicating whether the logger is enabled fur <see cref="EasyLogLevel.Info"/> messages
        /// </summary>
        bool IsInfoEnabled { get; }

        /// <summary>
        /// Flag indicating whether the logger is enabled fur <see cref="EasyLogLevel.Warn"/> messages
        /// </summary>
        bool IsWarnEnabled { get; }

        /// <summary>
        /// Flag indicating whether the logger is enabled fur <see cref="EasyLogLevel.Error"/> messages
        /// </summary>
        bool IsErrorEnabled { get; }

        /// <summary>
        /// Flag indicating whether the logger is enabled fur <see cref="EasyLogLevel.Fatal"/> messages
        /// </summary>
        bool IsFatalEnabled { get; }
    }
}