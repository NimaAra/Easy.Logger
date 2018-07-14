namespace Easy.Logger
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.Text;
    using Easy.Logger.Interfaces;
    using log4net;
    using log4net.Core;
    using log4net.Util;

    /// <summary>
    /// A <c>log4net</c> implementation of the <see cref="IEasyLogger{T}"/> interface.
    /// </summary>
    public sealed class Log4NetLogger<T> : Log4NetLogger, IEasyLogger<T>
    {
        static Log4NetLogger() => _ = Log4NetService.Instance;

        /// <summary>
        /// Creates an instance of the <see cref="Log4NetLogger{T}"/> where the name of the logger is
        /// set as the name of the type of <typeparamref name="T"/>.
        /// </summary>
        public Log4NetLogger() : base(LogManager.GetLogger(typeof(T))) {}
    }
    
    /// <summary>
    /// A <c>log4net</c> implementation of the <see cref="ILogger"/> interface.
    /// </summary>
    public class Log4NetLogger : IEasyLogger
    {
		private static readonly Type ThisDeclaringType = typeof(Log4NetLogger);
        private readonly ILog _logger;

        /// <summary>
        /// Creates an instance of the <see cref="Log4NetLogger"/>.
        /// </summary>
        /// <param name="logger"></param>
        protected internal Log4NetLogger(ILog logger) => _logger = logger; 

        /// <summary>
        /// Gets the logger name.
        /// </summary>
        public string Name => _logger.Logger.Name;

        /// <summary>
        /// Returns an <see cref="IDisposable"/> which allows the caller to specify a scope as
        /// <paramref name="name"/> which will then be rendered as part of the message.
        /// </summary>
        /// <param name="name">The name of the scope</param>
        public IDisposable GetScopedLogger(string name) => new Scope(name);

    #region Levels Enabled

        /// <summary>
        /// Gets the flag indicating whether the logger is enabled for 
        /// <c>Trace</c> messages.
        /// </summary>
        public bool IsTraceEnabled => IsEnabledFor(Level.Trace);

        /// <summary>
        /// Gets the flag indicating whether the logger is enabled for 
        /// <c>Debug</c> messages.
        /// </summary>
        public bool IsDebugEnabled => _logger.IsDebugEnabled;

        /// <summary>
        /// Gets the flag indicating whether the logger is enabled for 
        /// <c>Info</c> messages.
        /// </summary>
        public bool IsInfoEnabled => _logger.IsInfoEnabled;

        /// <summary>
        /// Gets the flag indicating whether the logger is enabled for 
        /// <c>Warn</c> messages.
        /// </summary>
        public bool IsWarnEnabled => _logger.IsWarnEnabled;

        /// <summary>
        /// Gets the flag indicating whether the logger is enabled for 
        /// <c>Error</c> messages.
        /// </summary>
        public bool IsErrorEnabled => _logger.IsErrorEnabled;

        /// <summary>
        /// Gets the flag indicating whether the logger is enabled for 
        /// <c>Fatal</c> messages.
        /// </summary>
        public bool IsFatalEnabled => _logger.IsFatalEnabled;

    #endregion

    #region Trace
        
        /// <summary>
        /// Logs a message object with the <c>Trace</c> level.
        /// </summary>
        /// <param name="message">The message object to be logged.</param>
        /// <remarks>
        /// <para>
        /// This method first checks if this logger is <c>Debug</c> enabled by comparing the level of 
        /// this logger with the <c>Trace</c> level. If this logger is <c>Debug</c> 
        /// enabled, then it converts the message object (passed as parameter) to a string by invoking the appropriate
        /// <see cref="T:log4net.ObjectRenderer.IObjectRenderer"/>. It then proceeds to call all the registered appenders 
        /// in this logger  and also higher in the hierarchy depending on the value of the additivity flag.
        /// </para>
        /// <para>
        /// <b>WARNING</b> Note that passing an<see cref="T:System.Exception"/> to this method 
        /// will print the name of the <see cref="T:System.Exception"/> but no stack trace.
        /// To print a stack trace use the <see cref="M:Debug(object,Exception)"/> form instead.
        /// </para>
        /// </remarks>
        [DebuggerStepThrough]
        public void Trace(object message) => LogImpl(Level.Trace, message, null);

        /// <summary>
        /// Logs a message object with the <c>Trace</c> level including 
        /// the stack trace of the <see cref="T:System.Exception"/> passed as a parameter.
        /// </summary>
        /// <param name="message">The message object to be logged.</param>
        /// <param name="exception">The exception to be logged, including its stack trace.</param>
        [DebuggerStepThrough]
        public void Trace(object message, Exception exception) => LogImpl(Level.Trace, message, exception);

        /// <summary>
        /// Logs a formatted message string with the <c>Trace</c> level.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="arg">The object to format</param>
        /// <remarks>
        /// <para>
        /// The message is formatted using the <c>String.Format</c> method. 
        /// See <see cref = "M:String.Format(string, object[])" /> for details 
        /// of the syntax of the format string and the behavior of the formatting.
        /// </para>
        /// <para>
        /// This method does not take an <see cref= "T:System.Exception"/> object to include in the log event. 
        /// To pass an <see cref="T:System.Exception"/> use one of the <see cref="M:Debug(object,Exception)"/> methods instead.
        /// </para>
        /// </remarks>
        [DebuggerStepThrough]
        public void TraceFormat(string format, object arg) 
            => LogImpl(Level.Trace, new SystemStringFormat(CultureInfo.InvariantCulture, format, arg), null);

        /// <summary>
        /// Logs a formatted message string with the <c>Trace</c> level.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="arg1">The first object to format</param>
        /// <param name="arg2">The second object to format</param>
        /// <remarks>
        /// <para>
        /// This method does not take an <see cref= "T:System.Exception"/> object to include in the log event. 
        /// To pass an <see cref="T:System.Exception"/> use one of the <see cref="M:Debug(object,Exception)"/> methods instead.
        /// </para>
        /// </remarks>
        [DebuggerStepThrough]
        public void TraceFormat(string format, object arg1, object arg2) 
            => LogImpl(
                Level.Trace, 
                new SystemStringFormat(CultureInfo.InvariantCulture, format, arg1, arg2), 
                null);

        /// <summary>
        /// Logs a formatted message string with the <c>Trace</c> level.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="arg1">The first object to format</param>
        /// <param name="arg2">The second object to format</param>
        /// <param name="arg3">The third object to format</param>
        /// <remarks>
        /// <para>
        /// This method does not take an <see cref= "T:System.Exception"/> object to include in the log event. 
        /// To pass an <see cref="T:System.Exception"/> use one of the <see cref="M:Debug(object,Exception)"/> methods instead.
        /// </para>
        /// </remarks>
        [DebuggerStepThrough]
        public void TraceFormat(string format, object arg1, object arg2, object arg3) 
            => LogImpl(
                Level.Trace, 
                new SystemStringFormat(CultureInfo.InvariantCulture, format, arg1, arg2, arg3),
                null);

        /// <summary>
        /// Logs a formatted message string with the <c>Trace</c> level.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="args">An Object array containing zero or more objects to format</param>
        /// <remarks>
        /// <para>
        /// The message is formatted using the <c>String.Format</c> method. See <see cref="M:String.Format(string, object[])"/> 
        /// for details of the syntax of the format string and the behavior of the formatting.
        /// </para>
        /// <para>
        /// This method does not take an <see cref = "T:System.Exception"/> object to include in the log event. 
        /// To pass an<see cref="T:System.Exception"/> use one of the <see cref="M:Debug(object,Exception)"/> methods instead.
        /// </para>
        /// </remarks>
        [DebuggerStepThrough]
        public void TraceFormat(string format, params object[] args)
            => LogImpl(
                Level.Trace, 
                new SystemStringFormat(CultureInfo.InvariantCulture, format, args),
                null);

        /// <summary>
        /// Logs a formatted message string with the <c>Trace</c> level.
        /// </summary>
        /// <param name="provider">An <see cref= "T:System.IFormatProvider" /> that supplies culture-specific formatting information</param>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="args">An Object array containing zero or more objects to format</param>
        /// <remarks>
        /// <para>
        /// The message is formatted using the <c>String.Format</c> method. See <see cref="M:String.Format(string, object[])"/> 
        /// for details of the syntax of the format string and the behavior of the formatting.
        /// </para>
        /// <para>
        /// This method does not take an <see cref="T:System.Exception"/> object to include in the log event. 
        /// To pass an <see cref="T:System.Exception"/>, use one of the <see cref="M:Debug(object,Exception)"/> methods instead.
        /// </para>
        /// </remarks>
        [DebuggerStepThrough]
        public void TraceFormat(IFormatProvider provider, string format, params object[] args) 
            => LogImpl(
                Level.Trace, 
                new SystemStringFormat(provider, format, args),
                null);
#endregion

    #region Debug

        /// <summary>
        /// Logs a message object with the <c>Debug</c> level.
        /// </summary>
        /// <param name="message">The message object to be logged.</param>
        /// <remarks>
        /// <para>
        /// This method first checks if this logger is <c>Debug</c> enabled by comparing the level of 
        /// this logger with the <c>Debug</c> level. If this logger is <c>Debug</c> 
        /// enabled, then it converts the message object (passed as parameter) to a string by invoking the appropriate
        /// <see cref="T:log4net.ObjectRenderer.IObjectRenderer"/>. It then proceeds to call all the registered appenders 
        /// in this logger  and also higher in the hierarchy depending on the value of the additivity flag.
        /// </para>
        /// <para>
        /// <b>WARNING</b> Note that passing an<see cref="T:System.Exception"/> to this method 
        /// will print the name of the <see cref="T:System.Exception"/> but no stack trace.
        /// To print a stack trace use the <see cref="M:Debug(object,Exception)"/> form instead.
        /// </para>
        /// </remarks>
        [DebuggerStepThrough]
        public void Debug(object message) => LogImpl(Level.Debug, message, null);

        /// <summary>
        /// Logs a message object with the <c>Debug</c> level including 
        /// the stack trace of the <see cref="T:System.Exception"/> passed as a parameter.
        /// </summary>
        /// <param name="message">The message object to be logged.</param>
        /// <param name="exception">The exception to be logged, including its stack trace.</param>
        [DebuggerStepThrough]
        public void Debug(object message, Exception exception) => LogImpl(Level.Debug, message, exception);

        /// <summary>
        /// Logs a formatted message string with the <c>Debug</c> level.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="arg">The object to format</param>
        /// <remarks>
        /// <para>
        /// The message is formatted using the <c>String.Format</c> method. 
        /// See <see cref = "M:String.Format(string, object[])" /> for details 
        /// of the syntax of the format string and the behavior of the formatting.
        /// </para>
        /// <para>
        /// This method does not take an <see cref= "T:System.Exception"/> object to include in the log event. 
        /// To pass an <see cref="T:System.Exception"/> use one of the <see cref="M:Debug(object,Exception)"/> methods instead.
        /// </para>
        /// </remarks>
        [DebuggerStepThrough]
        public void DebugFormat(string format, object arg) 
            => LogImpl(Level.Debug, new SystemStringFormat(CultureInfo.InvariantCulture, format, arg), null);

        /// <summary>
        /// Logs a formatted message string with the <c>Debug</c> level.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="arg1">The first object to format</param>
        /// <param name="arg2">The second object to format</param>
        /// <remarks>
        /// <para>
        /// This method does not take an <see cref= "T:System.Exception"/> object to include in the log event. 
        /// To pass an <see cref="T:System.Exception"/> use one of the <see cref="M:Debug(object,Exception)"/> methods instead.
        /// </para>
        /// </remarks>
        [DebuggerStepThrough]
        public void DebugFormat(string format, object arg1, object arg2) 
            => LogImpl(
                Level.Debug, 
                new SystemStringFormat(CultureInfo.InvariantCulture, format, arg1, arg2), 
                null);

        /// <summary>
        /// Logs a formatted message string with the <c>Debug</c> level.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="arg1">The first object to format</param>
        /// <param name="arg2">The second object to format</param>
        /// <param name="arg3">The third object to format</param>
        /// <remarks>
        /// <para>
        /// This method does not take an <see cref= "T:System.Exception"/> object to include in the log event. 
        /// To pass an <see cref="T:System.Exception"/> use one of the <see cref="M:Debug(object,Exception)"/> methods instead.
        /// </para>
        /// </remarks>
        [DebuggerStepThrough]
        public void DebugFormat(string format, object arg1, object arg2, object arg3) 
            => LogImpl(
                Level.Debug, 
                new SystemStringFormat(CultureInfo.InvariantCulture, format, arg1, arg2, arg3),
                null);

        /// <summary>
        /// Logs a formatted message string with the <c>Debug</c> level.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="args">An Object array containing zero or more objects to format</param>
        /// <remarks>
        /// <para>
        /// The message is formatted using the <c>String.Format</c> method. See <see cref="M:String.Format(string, object[])"/> 
        /// for details of the syntax of the format string and the behavior of the formatting.
        /// </para>
        /// <para>
        /// This method does not take an <see cref = "T:System.Exception"/> object to include in the log event. 
        /// To pass an<see cref="T:System.Exception"/> use one of the <see cref="M:Debug(object,Exception)"/> methods instead.
        /// </para>
        /// </remarks>
        [DebuggerStepThrough]
        public void DebugFormat(string format, params object[] args) 
            => LogImpl(
                Level.Debug, 
                new SystemStringFormat(CultureInfo.InvariantCulture, format, args),
                null);

        /// <summary>
        /// Logs a formatted message string with the <c>Debug</c> level.
        /// </summary>
        /// <param name="provider">An <see cref= "T:System.IFormatProvider" /> that supplies culture-specific formatting information</param>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="args">An Object array containing zero or more objects to format</param>
        /// <remarks>
        /// <para>
        /// The message is formatted using the <c>String.Format</c> method. See <see cref="M:String.Format(string, object[])"/> 
        /// for details of the syntax of the format string and the behavior of the formatting.
        /// </para>
        /// <para>
        /// This method does not take an <see cref="T:System.Exception"/> object to include in the log event. 
        /// To pass an <see cref="T:System.Exception"/>, use one of the <see cref="M:Debug(object,Exception)"/> methods instead.
        /// </para>
        /// </remarks>
        [DebuggerStepThrough]
        public void DebugFormat(IFormatProvider provider, string format, params object[] args) 
            => LogImpl(
                Level.Debug, 
                new SystemStringFormat(provider, format, args),
                null);

    #endregion

    #region Info
            
        /// <summary>
        /// Logs a message object with the <c>Info</c> level.
        /// </summary>
        /// <param name="message">The message object to be logged.</param>
        /// <remarks>
        /// <para>
        /// This method first checks if this logger is <c>Info</c> enabled by comparing the level of 
        /// this logger with the <c>Info</c> level. If this logger is <c>Info</c> 
        /// enabled, then it converts the message object (passed as parameter) to a string by invoking the appropriate
        /// <see cref="T:log4net.ObjectRenderer.IObjectRenderer"/>. It then proceeds to call all the registered appenders 
        /// in this logger  and also higher in the hierarchy depending on the value of the additivity flag.
        /// </para>
        /// <para>
        /// <b>WARNING</b> Note that passing an<see cref="T:System.Exception"/> to this method 
        /// will print the name of the <see cref="T:System.Exception"/> but no stack trace.
        /// To print a stack trace use the <see cref="M:Info(object,Exception)"/> form instead.
        /// </para>
        /// </remarks>
        [DebuggerStepThrough]
        public void Info(object message) => LogImpl(Level.Info, message, null);

        /// <summary>
        /// Logs a message object with the <c>Info</c> level including 
        /// the stack trace of the <see cref="T:System.Exception"/> passed as a parameter.
        /// </summary>
        /// <param name="message">The message object to be logged.</param>
        /// <param name="exception">The exception to be logged, including its stack trace.</param>
        [DebuggerStepThrough]
        public void Info(object message, Exception exception) => LogImpl(Level.Info, message, exception);

        /// <summary>
        /// Logs a formatted message string with the <c>Info</c> level.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="arg">The object to format</param>
        /// <remarks>
        /// <para>
        /// The message is formatted using the <c>String.Format</c> method. 
        /// See <see cref = "M:String.Format(string, object[])" /> for details 
        /// of the syntax of the format string and the behavior of the formatting.
        /// </para>
        /// <para>
        /// This method does not take an <see cref= "T:System.Exception"/> object to include in the log event. 
        /// To pass an <see cref="T:System.Exception"/> use one of the <see cref="M:Info(object,Exception)"/> methods instead.
        /// </para>
        /// </remarks>
        [DebuggerStepThrough]
        public void InfoFormat(string format, object arg) 
            => LogImpl(Level.Info, new SystemStringFormat(CultureInfo.InvariantCulture, format, arg), null);

        /// <summary>
        /// Logs a formatted message string with the <c>Info</c> level.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="arg1">The first object to format</param>
        /// <param name="arg2">The second object to format</param>
        /// <remarks>
        /// <para>
        /// This method does not take an <see cref= "T:System.Exception"/> object to include in the log event. 
        /// To pass an <see cref="T:System.Exception"/> use one of the <see cref="M:Info(object,Exception)"/> methods instead.
        /// </para>
        /// </remarks>
        [DebuggerStepThrough]
        public void InfoFormat(string format, object arg1, object arg2) 
            => LogImpl(
                Level.Info, 
                new SystemStringFormat(CultureInfo.InvariantCulture, format, arg1, arg2), 
                null);

        /// <summary>
        /// Logs a formatted message string with the <c>Info</c> level.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="arg1">The first object to format</param>
        /// <param name="arg2">The second object to format</param>
        /// <param name="arg3">The third object to format</param>
        /// <remarks>
        /// <para>
        /// This method does not take an <see cref= "T:System.Exception"/> object to include in the log event. 
        /// To pass an <see cref="T:System.Exception"/> use one of the <see cref="M:Info(object,Exception)"/> methods instead.
        /// </para>
        /// </remarks>
        [DebuggerStepThrough]
        public void InfoFormat(string format, object arg1, object arg2, object arg3) 
            => LogImpl(
                Level.Info, 
                new SystemStringFormat(CultureInfo.InvariantCulture, format, arg1, arg2, arg3),
                null);

        /// <summary>
        /// Logs a formatted message string with the <c>Info</c> level.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="args">An Object array containing zero or more objects to format</param>
        /// <remarks>
        /// <para>
        /// The message is formatted using the <c>String.Format</c> method. See <see cref="M:String.Format(string, object[])"/> 
        /// for details of the syntax of the format string and the behavior of the formatting.
        /// </para>
        /// <para>
        /// This method does not take an <see cref = "T:System.Exception"/> object to include in the log event. 
        /// To pass an<see cref="T:System.Exception"/> use one of the <see cref="M:Info(object,Exception)"/> methods instead.
        /// </para>
        /// </remarks>
        [DebuggerStepThrough]
        public void InfoFormat(string format, params object[] args) 
            => LogImpl(
                Level.Info, 
                new SystemStringFormat(CultureInfo.InvariantCulture, format, args),
                null);

        /// <summary>
        /// Logs a formatted message string with the <c>Info</c> level.
        /// </summary>
        /// <param name="provider">An <see cref= "T:System.IFormatProvider" /> that supplies culture-specific formatting information</param>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="args">An Object array containing zero or more objects to format</param>
        /// <remarks>
        /// <para>
        /// The message is formatted using the <c>String.Format</c> method. See <see cref="M:String.Format(string, object[])"/> 
        /// for details of the syntax of the format string and the behavior of the formatting.
        /// </para>
        /// <para>
        /// This method does not take an <see cref="T:System.Exception"/> object to include in the log event. 
        /// To pass an <see cref="T:System.Exception"/>, use one of the <see cref="M:Info(object,Exception)"/> methods instead.
        /// </para>
        /// </remarks>
        [DebuggerStepThrough]
        public void InfoFormat(IFormatProvider provider, string format, params object[] args) 
            => LogImpl(
                Level.Info, 
                new SystemStringFormat(provider, format, args),
                null);

    #endregion

    #region Warn
            
        /// <summary>
        /// Logs a message object with the <c>Warn</c> level.
        /// </summary>
        /// <param name="message">The message object to be logged.</param>
        /// <remarks>
        /// <para>
        /// This method first checks if this logger is <c>Warn</c> enabled by comparing the level of 
        /// this logger with the <c>Warn</c> level. If this logger is <c>Warn</c> 
        /// enabled, then it converts the message object (passed as parameter) to a string by invoking the appropriate
        /// <see cref="T:log4net.ObjectRenderer.IObjectRenderer"/>. It then proceeds to call all the registered appenders 
        /// in this logger  and also higher in the hierarchy depending on the value of the additivity flag.
        /// </para>
        /// <para>
        /// <b>WARNING</b> Note that passing an<see cref="T:System.Exception"/> to this method 
        /// will print the name of the <see cref="T:System.Exception"/> but no stack trace.
        /// To print a stack trace use the <see cref="M:Warn(object,Exception)"/> form instead.
        /// </para>
        /// </remarks>
        [DebuggerStepThrough]
        public void Warn(object message) => LogImpl(Level.Warn, message, null);

        /// <summary>
        /// Logs a message object with the <c>Warn</c> level including 
        /// the stack trace of the <see cref="T:System.Exception"/> passed as a parameter.
        /// </summary>
        /// <param name="message">The message object to be logged.</param>
        /// <param name="exception">The exception to be logged, including its stack trace.</param>
        [DebuggerStepThrough]
        public void Warn(object message, Exception exception) => LogImpl(Level.Warn, message, exception);

        /// <summary>
        /// Logs a formatted message string with the <c>Warn</c> level.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="arg">The object to format</param>
        /// <remarks>
        /// <para>
        /// The message is formatted using the <c>String.Format</c> method. 
        /// See <see cref = "M:String.Format(string, object[])" /> for details 
        /// of the syntax of the format string and the behavior of the formatting.
        /// </para>
        /// <para>
        /// This method does not take an <see cref= "T:System.Exception"/> object to include in the log event. 
        /// To pass an <see cref="T:System.Exception"/> use one of the <see cref="M:Warn(object,Exception)"/> methods instead.
        /// </para>
        /// </remarks>
        [DebuggerStepThrough]
        public void WarnFormat(string format, object arg) 
            => LogImpl(Level.Warn, new SystemStringFormat(CultureInfo.InvariantCulture, format, arg), null);

        /// <summary>
        /// Logs a formatted message string with the <c>Warn</c> level.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="arg1">The first object to format</param>
        /// <param name="arg2">The second object to format</param>
        /// <remarks>
        /// <para>
        /// This method does not take an <see cref= "T:System.Exception"/> object to include in the log event. 
        /// To pass an <see cref="T:System.Exception"/> use one of the <see cref="M:Warn(object,Exception)"/> methods instead.
        /// </para>
        /// </remarks>
        [DebuggerStepThrough]
        public void WarnFormat(string format, object arg1, object arg2) 
            => LogImpl(
                Level.Warn, 
                new SystemStringFormat(CultureInfo.InvariantCulture, format, arg1, arg2), 
                null);

        /// <summary>
        /// Logs a formatted message string with the <c>Warn</c> level.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="arg1">The first object to format</param>
        /// <param name="arg2">The second object to format</param>
        /// <param name="arg3">The third object to format</param>
        /// <remarks>
        /// <para>
        /// This method does not take an <see cref= "T:System.Exception"/> object to include in the log event. 
        /// To pass an <see cref="T:System.Exception"/> use one of the <see cref="M:Warn(object,Exception)"/> methods instead.
        /// </para>
        /// </remarks>
        [DebuggerStepThrough]
        public void WarnFormat(string format, object arg1, object arg2, object arg3) 
            => LogImpl(
                Level.Warn, 
                new SystemStringFormat(CultureInfo.InvariantCulture, format, arg1, arg2, arg3),
                null);

        /// <summary>
        /// Logs a formatted message string with the <c>Warn</c> level.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="args">An Object array containing zero or more objects to format</param>
        /// <remarks>
        /// <para>
        /// The message is formatted using the <c>String.Format</c> method. See <see cref="M:String.Format(string, object[])"/> 
        /// for details of the syntax of the format string and the behavior of the formatting.
        /// </para>
        /// <para>
        /// This method does not take an <see cref = "T:System.Exception"/> object to include in the log event. 
        /// To pass an<see cref="T:System.Exception"/> use one of the <see cref="M:Warn(object,Exception)"/> methods instead.
        /// </para>
        /// </remarks>
        [DebuggerStepThrough]
        public void WarnFormat(string format, params object[] args) 
            => LogImpl(
                Level.Warn, 
                new SystemStringFormat(CultureInfo.InvariantCulture, format, args),
                null);

        /// <summary>
        /// Logs a formatted message string with the <c>Warn</c> level.
        /// </summary>
        /// <param name="provider">An <see cref= "T:System.IFormatProvider" /> that supplies culture-specific formatting information</param>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="args">An Object array containing zero or more objects to format</param>
        /// <remarks>
        /// <para>
        /// The message is formatted using the <c>String.Format</c> method. See <see cref="M:String.Format(string, object[])"/> 
        /// for details of the syntax of the format string and the behavior of the formatting.
        /// </para>
        /// <para>
        /// This method does not take an <see cref="T:System.Exception"/> object to include in the log event. 
        /// To pass an <see cref="T:System.Exception"/>, use one of the <see cref="M:Warn(object,Exception)"/> methods instead.
        /// </para>
        /// </remarks>
        [DebuggerStepThrough]
        public void WarnFormat(IFormatProvider provider, string format, params object[] args) => LogImpl(
            Level.Warn, 
            new SystemStringFormat(provider, format, args),
            null);

    #endregion

    #region Error

        /// <summary>
        /// Logs a message object with the <c>Error</c> level.
        /// </summary>
        /// <param name="message">The message object to be logged.</param>
        /// <remarks>
        /// <para>
        /// This method first checks if this logger is <c>Error</c> enabled by comparing the level of 
        /// this logger with the <c>Error</c> level. If this logger is <c>Error</c> 
        /// enabled, then it converts the message object (passed as parameter) to a string by invoking the appropriate
        /// <see cref="T:log4net.ObjectRenderer.IObjectRenderer"/>. It then proceeds to call all the registered appenders 
        /// in this logger  and also higher in the hierarchy depending on the value of the additivity flag.
        /// </para>
        /// <para>
        /// <b>WARNING</b> Note that passing an<see cref="T:System.Exception"/> to this method 
        /// will print the name of the <see cref="T:System.Exception"/> but no stack trace.
        /// To print a stack trace use the <see cref="M:Error(object,Exception)"/> form instead.
        /// </para>
        /// </remarks>
        [DebuggerStepThrough]
        public void Error(object message) => LogImpl(Level.Error, message, null);

        /// <summary>
        /// Logs a message object with the <c>Error</c> level including 
        /// the stack trace of the <see cref="T:System.Exception"/> passed as a parameter.
        /// </summary>
        /// <param name="message">The message object to be logged.</param>
        /// <param name="exception">The exception to be logged, including its stack trace.</param>
        [DebuggerStepThrough]
        public void Error(object message, Exception exception) => LogImpl(Level.Error, message, exception);

        /// <summary>
        /// Logs a formatted message string with the <c>Error</c> level.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="arg">The object to format</param>
        /// <remarks>
        /// <para>
        /// The message is formatted using the <c>String.Format</c> method. 
        /// See <see cref = "M:String.Format(string, object[])" /> for details 
        /// of the syntax of the format string and the behavior of the formatting.
        /// </para>
        /// <para>
        /// This method does not take an <see cref= "T:System.Exception"/> object to include in the log event. 
        /// To pass an <see cref="T:System.Exception"/> use one of the <see cref="M:Error(object,Exception)"/> methods instead.
        /// </para>
        /// </remarks>
        [DebuggerStepThrough]
        public void ErrorFormat(string format, object arg) 
            => LogImpl(Level.Error, new SystemStringFormat(CultureInfo.InvariantCulture, format, arg), null);

        /// <summary>
        /// Logs a formatted message string with the <c>Error</c> level.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="arg1">The first object to format</param>
        /// <param name="arg2">The second object to format</param>
        /// <remarks>
        /// <para>
        /// This method does not take an <see cref= "T:System.Exception"/> object to include in the log event. 
        /// To pass an <see cref="T:System.Exception"/> use one of the <see cref="M:Error(object,Exception)"/> methods instead.
        /// </para>
        /// </remarks>
        [DebuggerStepThrough]
        public void ErrorFormat(string format, object arg1, object arg2) 
            => LogImpl(
                Level.Error, 
                new SystemStringFormat(CultureInfo.InvariantCulture, format, arg1, arg2), 
                null);

        /// <summary>
        /// Logs a formatted message string with the <c>Error</c> level.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="arg1">The first object to format</param>
        /// <param name="arg2">The second object to format</param>
        /// <param name="arg3">The third object to format</param>
        /// <remarks>
        /// <para>
        /// This method does not take an <see cref= "T:System.Exception"/> object to include in the log event. 
        /// To pass an <see cref="T:System.Exception"/> use one of the <see cref="M:Error(object,Exception)"/> methods instead.
        /// </para>
        /// </remarks>
        [DebuggerStepThrough]
        public void ErrorFormat(string format, object arg1, object arg2, object arg3) 
            => LogImpl(
                Level.Error, 
                new SystemStringFormat(CultureInfo.InvariantCulture, format, arg1, arg2, arg3),
                null);

        /// <summary>
        /// Logs a formatted message string with the <c>Error</c> level.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="args">An Object array containing zero or more objects to format</param>
        /// <remarks>
        /// <para>
        /// The message is formatted using the <c>String.Format</c> method. See <see cref="M:String.Format(string, object[])"/> 
        /// for details of the syntax of the format string and the behavior of the formatting.
        /// </para>
        /// <para>
        /// This method does not take an <see cref = "T:System.Exception"/> object to include in the log event. 
        /// To pass an<see cref="T:System.Exception"/> use one of the <see cref="M:Error(object,Exception)"/> methods instead.
        /// </para>
        /// </remarks>
        [DebuggerStepThrough]
        public void ErrorFormat(string format, params object[] args) 
            => LogImpl(
                Level.Error, 
                new SystemStringFormat(CultureInfo.InvariantCulture, format, args),
                null);

        /// <summary>
        /// Logs a formatted message string with the <c>Error</c> level.
        /// </summary>
        /// <param name="provider">An <see cref= "T:System.IFormatProvider" /> that supplies culture-specific formatting information</param>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="args">An Object array containing zero or more objects to format</param>
        /// <remarks>
        /// <para>
        /// The message is formatted using the <c>String.Format</c> method. See <see cref="M:String.Format(string, object[])"/> 
        /// for details of the syntax of the format string and the behavior of the formatting.
        /// </para>
        /// <para>
        /// This method does not take an <see cref="T:System.Exception"/> object to include in the log event. 
        /// To pass an <see cref="T:System.Exception"/>, use one of the <see cref="M:Error(object,Exception)"/> methods instead.
        /// </para>
        /// </remarks>
        [DebuggerStepThrough]
        public void ErrorFormat(IFormatProvider provider, string format, params object[] args) 
            => LogImpl(
                Level.Error, 
                new SystemStringFormat(provider, format, args),
                null);

    #endregion

    #region Fatal

        /// <summary>
        /// Logs a message object with the <c>Fatal</c> level.
        /// </summary>
        /// <param name="message">The message object to be logged.</param>
        /// <remarks>
        /// <para>
        /// This method first checks if this logger is <c>Fatal</c> enabled by comparing the level of 
        /// this logger with the <c>Fatal</c> level. If this logger is <c>Fatal</c> 
        /// enabled, then it converts the message object (passed as parameter) to a string by invoking the appropriate
        /// <see cref="T:log4net.ObjectRenderer.IObjectRenderer"/>. It then proceeds to call all the registered appenders 
        /// in this logger  and also higher in the hierarchy depending on the value of the additivity flag.
        /// </para>
        /// <para>
        /// <b>WARNING</b> Note that passing an<see cref="T:System.Exception"/> to this method 
        /// will print the name of the <see cref="T:System.Exception"/> but no stack trace.
        /// To print a stack trace use the <see cref="M:Fatal(object,Exception)"/> form instead.
        /// </para>
        /// </remarks>
        [DebuggerStepThrough]
        public void Fatal(object message) => LogImpl(Level.Fatal, message, null);

        /// <summary>
        /// Logs a message object with the <c>Fatal</c> level including 
        /// the stack trace of the <see cref="T:System.Exception"/> passed as a parameter.
        /// </summary>
        /// <param name="message">The message object to be logged.</param>
        /// <param name="exception">The exception to be logged, including its stack trace.</param>
        [DebuggerStepThrough]
        public void Fatal(object message, Exception exception) => LogImpl(Level.Fatal, message, exception);

        /// <summary>
        /// Logs a formatted message string with the <c>Fatal</c> level.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="arg">The object to format</param>
        /// <remarks>
        /// <para>
        /// The message is formatted using the <c>String.Format</c> method. 
        /// See <see cref = "M:String.Format(string, object[])" /> for details 
        /// of the syntax of the format string and the behavior of the formatting.
        /// </para>
        /// <para>
        /// This method does not take an <see cref= "T:System.Exception"/> object to include in the log event. 
        /// To pass an <see cref="T:System.Exception"/> use one of the <see cref="M:Fatal(object,Exception)"/> methods instead.
        /// </para>
        /// </remarks>
        [DebuggerStepThrough]
        public void FatalFormat(string format, object arg) 
            => LogImpl(Level.Fatal, new SystemStringFormat(CultureInfo.InvariantCulture, format, arg), null);

        /// <summary>
        /// Logs a formatted message string with the <c>Fatal</c> level.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="arg1">The first object to format</param>
        /// <param name="arg2">The second object to format</param>
        /// <remarks>
        /// <para>
        /// This method does not take an <see cref= "T:System.Exception"/> object to include in the log event. 
        /// To pass an <see cref="T:System.Exception"/> use one of the <see cref="M:Fatal(object,Exception)"/> methods instead.
        /// </para>
        /// </remarks>
        [DebuggerStepThrough]
        public void FatalFormat(string format, object arg1, object arg2) 
            => LogImpl(
                Level.Fatal, 
                new SystemStringFormat(CultureInfo.InvariantCulture, format, arg1, arg2), 
                null);

        /// <summary>
        /// Logs a formatted message string with the <c>Fatal</c> level.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="arg1">The first object to format</param>
        /// <param name="arg2">The second object to format</param>
        /// <param name="arg3">The third object to format</param>
        /// <remarks>
        /// <para>
        /// This method does not take an <see cref= "T:System.Exception"/> object to include in the log event. 
        /// To pass an <see cref="T:System.Exception"/> use one of the <see cref="M:Fatal(object,Exception)"/> methods instead.
        /// </para>
        /// </remarks>
        [DebuggerStepThrough]
        public void FatalFormat(string format, object arg1, object arg2, object arg3) 
            => LogImpl(
                Level.Fatal, 
                new SystemStringFormat(CultureInfo.InvariantCulture, format, arg1, arg2, arg3),
                null);

        /// <summary>
        /// Logs a formatted message string with the <c>Fatal</c> level.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="args">An Object array containing zero or more objects to format</param>
        /// <remarks>
        /// <para>
        /// The message is formatted using the <c>String.Format</c> method. See <see cref="M:String.Format(string, object[])"/> 
        /// for details of the syntax of the format string and the behavior of the formatting.
        /// </para>
        /// <para>
        /// This method does not take an <see cref = "T:System.Exception"/> object to include in the log event. 
        /// To pass an<see cref="T:System.Exception"/> use one of the <see cref="M:Fatal(object,Exception)"/> methods instead.
        /// </para>
        /// </remarks>
        [DebuggerStepThrough]
        public void FatalFormat(string format, params object[] args) 
            => LogImpl(
                Level.Fatal, 
                new SystemStringFormat(CultureInfo.InvariantCulture, format, args),
                null);

        /// <summary>
        /// Logs a formatted message string with the <c>Fatal</c> level.
        /// </summary>
        /// <param name="provider">An <see cref= "T:System.IFormatProvider" /> that supplies culture-specific formatting information</param>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="args">An Object array containing zero or more objects to format</param>
        /// <remarks>
        /// <para>
        /// The message is formatted using the <c>String.Format</c> method. See <see cref="M:String.Format(string, object[])"/> 
        /// for details of the syntax of the format string and the behavior of the formatting.
        /// </para>
        /// <para>
        /// This method does not take an <see cref="T:System.Exception"/> object to include in the log event. 
        /// To pass an <see cref="T:System.Exception"/>, use one of the <see cref="M:Fatal(object,Exception)"/> methods instead.
        /// </para>
        /// </remarks>
        [DebuggerStepThrough]
        public void FatalFormat(IFormatProvider provider, string format, params object[] args) 
            => LogImpl(
                Level.Fatal, 
                new SystemStringFormat(provider, format, args),
                null);

    #endregion

        private static string PrefixScopesIfAny(string message)
        {
            if (Scope.IsEmpty) { return message; }
            
            StringBuilder builder;

            var scopes = Scope.Scopes;
            lock (scopes)
            {
                if (scopes.Count == 1)
                {
                    return string.Concat(scopes[0], " ", message);
                }

                builder = StringBuilderCache.Acquire();

                // ReSharper disable once ForCanBeConvertedToForeach
                for (var i = 0; i < scopes.Count; i++)
                {
                    var scope = scopes[i];
                    builder.Append(scope).Append(' ');
                }
            }

            builder.Append(message);

            return StringBuilderCache.GetStringAndRelease(builder);
        }

        private bool IsEnabledFor(Level level) => _logger.Logger.IsEnabledFor(level);

        private void LogImpl(Level level, object message, Exception exception)
        {
            if (!IsEnabledFor(level)) { return; }

            _logger.Logger.Log(
                ThisDeclaringType, 
                level, 
                PrefixScopesIfAny(message is string msgStr ? msgStr : message.ToString()), 
                exception);
        }

        private static class StringBuilderCache
        {
            [ThreadStatic]
            private static StringBuilder _cache;

            [DebuggerStepThrough]
            public static StringBuilder Acquire()
            {
                var result = _cache;
                if (result == null) { return new StringBuilder(); }

                result.Clear();
                _cache = null; // of that if caller forgets to release and return it is not kept alive by this class
                return result;
            }

            [DebuggerStepThrough]
            public static string GetStringAndRelease(StringBuilder builder)
            {
                var result = builder.ToString();
                _cache = builder;
                return result;
            }
        }
    }
}
