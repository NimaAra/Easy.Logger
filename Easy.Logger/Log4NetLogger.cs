namespace Easy.Logger
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using Easy.Logger.Interfaces;
    using log4net;
    using log4net.Core;
    using log4net.Util;

    /// <summary>
    /// A <c>log4net</c> implementation of the <see cref="ILogger"/> interface
    /// </summary>
    internal sealed class Log4NetLogger : Interfaces.IEasyLogger
    {
		private static readonly Type ThisDeclaringType = typeof(Log4NetLogger);
        private static readonly Level TraceLevel = Level.Trace;
        private readonly ILog _logger;

        [DebuggerStepThrough]
        internal Log4NetLogger(ILog logger)
        {
            _logger = logger;
            Name = _logger.Logger.Name;
        }

        /// <summary>
        /// Gets the logger name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Logs a message object with the <see cref="System.Diagnostics.Trace"/> level.
        /// </summary>
        /// <param name="message">The message object to be logged.</param>
        /// <remarks>
        /// <para>
        /// This method first checks if this logger is <c>Debug</c> enabled by comparing the level of 
        /// this logger with the <see cref="System.Diagnostics.Trace"/> level. If this logger is <c>Debug</c> 
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
        public void Trace(object message)
        {
            _logger.Logger.Log(ThisDeclaringType, TraceLevel, message, null);
        }

        /// <summary>
        /// Logs a message object with the <see cref="System.Diagnostics.Trace"/> level including 
        /// the stack trace of the <see cref="T:System.Exception"/> passed as a parameter.
        /// </summary>
        /// <param name="message">The message object to be logged.</param>
        /// <param name="exception">The exception to be logged, including its stack trace.</param>
        [DebuggerStepThrough]
        public void Trace(object message, Exception exception)
        {
            _logger.Logger.Log(ThisDeclaringType, TraceLevel, message, exception);
        }

        /// <summary>
        /// Logs a formatted message string with the <see cref="System.Diagnostics.Trace"/> level.
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
        {
            _logger.Logger.Log(ThisDeclaringType, TraceLevel, new SystemStringFormat(CultureInfo.InvariantCulture, format, arg), null);
        }

        /// <summary>
        /// Logs a formatted message string with the <see cref="System.Diagnostics.Trace"/> level.
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
        {
            _logger.Logger.Log(ThisDeclaringType, TraceLevel, new SystemStringFormat(CultureInfo.InvariantCulture, format, arg1, arg2), null);
        }

        /// <summary>
        /// Logs a formatted message string with the <see cref="System.Diagnostics.Trace"/> level.
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
        {
            _logger.Logger.Log(ThisDeclaringType, TraceLevel, new SystemStringFormat(CultureInfo.InvariantCulture, format, arg1, arg2, arg3), null);
        }

        /// <summary>
        /// Logs a formatted message string with the <see cref="System.Diagnostics.Trace"/> level.
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
        {
            _logger.Logger.Log(ThisDeclaringType, TraceLevel, new SystemStringFormat(CultureInfo.InvariantCulture, format, args), null);
        }

        /// <summary>
        /// Logs a formatted message string with the <see cref="System.Diagnostics.Trace"/> level.
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
        {
            _logger.Logger.Log(ThisDeclaringType, TraceLevel, new SystemStringFormat(provider, format, args), null);
        }

        /// <summary>
        /// Logs a message object with the <see cref="System.Diagnostics.Debug"/> level.
        /// </summary>
        /// <param name="message">The message object to be logged.</param>
        /// <remarks>
        /// <para>
        /// This method first checks if this logger is <c>Debug</c> enabled by comparing the level of 
        /// this logger with the <see cref="System.Diagnostics.Debug"/> level. If this logger is <c>Debug</c> 
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
        public void Debug(object message)
        {
            _logger.Debug(message);
        }

        /// <summary>
        /// Logs a message object with the <see cref="System.Diagnostics.Debug"/> level including 
        /// the stack trace of the <see cref="T:System.Exception"/> passed as a parameter.
        /// </summary>
        /// <param name="message">The message object to be logged.</param>
        /// <param name="exception">The exception to be logged, including its stack trace.</param>
        [DebuggerStepThrough]
        public void Debug(object message, Exception exception)
        {
            _logger.Debug(message, exception);
        }

        /// <summary>
        /// Logs a formatted message string with the <see cref="System.Diagnostics.Debug"/> level.
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
        {
            _logger.DebugFormat(format, arg);
        }

        /// <summary>
        /// Logs a formatted message string with the <see cref="System.Diagnostics.Debug"/> level.
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
        {
            _logger.DebugFormat(format, arg1, arg2);
        }

        /// <summary>
        /// Logs a formatted message string with the <see cref="System.Diagnostics.Debug"/> level.
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
        {
            _logger.DebugFormat(format, arg1, arg2, arg3);
        }

        /// <summary>
        /// Logs a formatted message string with the <see cref="System.Diagnostics.Debug"/> level.
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
        {
            _logger.DebugFormat(format, args);
        }

        /// <summary>
        /// Logs a formatted message string with the <see cref="System.Diagnostics.Debug"/> level.
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
        {
            _logger.DebugFormat(provider, format, args);
        }

        /// <summary>
        /// Logs a message object with the <see cref="EasyLogLevel.Info"/> level.
        /// </summary>
        /// <param name="message">The message object to be logged.</param>
        /// <remarks>
        /// <para>
        /// This method first checks if this logger is <c>Info</c> enabled by comparing the level of 
        /// this logger with the <see cref="EasyLogLevel.Info"/> level. If this logger is <c>Info</c> 
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
        public void Info(object message)
        {
            _logger.Info(message);
        }

        /// <summary>
        /// Logs a message object with the <see cref="EasyLogLevel.Info"/> level including 
        /// the stack trace of the <see cref="T:System.Exception"/> passed as a parameter.
        /// </summary>
        /// <param name="message">The message object to be logged.</param>
        /// <param name="exception">The exception to be logged, including its stack trace.</param>
        [DebuggerStepThrough]
        public void Info(object message, Exception exception)
        {
            _logger.Info(message, exception);
        }

        /// <summary>
        /// Logs a formatted message string with the <see cref="EasyLogLevel.Info"/> level.
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
        {
            _logger.InfoFormat(format, arg);
        }

        /// <summary>
        /// Logs a formatted message string with the <see cref="EasyLogLevel.Info"/> level.
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
        {
            _logger.InfoFormat(format, arg1, arg2);
        }

        /// <summary>
        /// Logs a formatted message string with the <see cref="EasyLogLevel.Info"/> level.
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
        {
            _logger.InfoFormat(format, arg1, arg2, arg3);
        }

        /// <summary>
        /// Logs a formatted message string with the <see cref="EasyLogLevel.Info"/> level.
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
        {
            _logger.InfoFormat(format, args);
        }

        /// <summary>
        /// Logs a formatted message string with the <see cref="EasyLogLevel.Info"/> level.
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
        {
            _logger.InfoFormat(provider, format, args);
        }

        /// <summary>
        /// Logs a message object with the <see cref="EasyLogLevel.Warn"/> level.
        /// </summary>
        /// <param name="message">The message object to be logged.</param>
        /// <remarks>
        /// <para>
        /// This method first checks if this logger is <c>Warn</c> enabled by comparing the level of 
        /// this logger with the <see cref="EasyLogLevel.Warn"/> level. If this logger is <c>Warn</c> 
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
        public void Warn(object message)
        {
            _logger.Warn(message);
        }

        /// <summary>
        /// Logs a message object with the <see cref="EasyLogLevel.Warn"/> level including 
        /// the stack trace of the <see cref="T:System.Exception"/> passed as a parameter.
        /// </summary>
        /// <param name="message">The message object to be logged.</param>
        /// <param name="exception">The exception to be logged, including its stack trace.</param>
        [DebuggerStepThrough]
        public void Warn(object message, Exception exception)
        {
            _logger.Warn(message, exception);
        }

        /// <summary>
        /// Logs a formatted message string with the <see cref="EasyLogLevel.Warn"/> level.
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
        {
            _logger.WarnFormat(format, arg);
        }

        /// <summary>
        /// Logs a formatted message string with the <see cref="EasyLogLevel.Warn"/> level.
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
        {
            _logger.WarnFormat(format, arg1, arg2);
        }

        /// <summary>
        /// Logs a formatted message string with the <see cref="EasyLogLevel.Warn"/> level.
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
        {
            _logger.WarnFormat(format, arg1, arg2, arg3);
        }

        /// <summary>
        /// Logs a formatted message string with the <see cref="EasyLogLevel.Warn"/> level.
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
        {
            _logger.WarnFormat(format, args);
        }

        /// <summary>
        /// Logs a formatted message string with the <see cref="EasyLogLevel.Warn"/> level.
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
        public void WarnFormat(IFormatProvider provider, string format, params object[] args)
        {
            _logger.WarnFormat(provider, format, args);
        }

        /// <summary>
        /// Logs a message object with the <see cref="EasyLogLevel.Error"/> level.
        /// </summary>
        /// <param name="message">The message object to be logged.</param>
        /// <remarks>
        /// <para>
        /// This method first checks if this logger is <c>Error</c> enabled by comparing the level of 
        /// this logger with the <see cref="EasyLogLevel.Error"/> level. If this logger is <c>Error</c> 
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
        public void Error(object message)
        {
            _logger.Error(message);
        }

        /// <summary>
        /// Logs a message object with the <see cref="EasyLogLevel.Error"/> level including 
        /// the stack trace of the <see cref="T:System.Exception"/> passed as a parameter.
        /// </summary>
        /// <param name="message">The message object to be logged.</param>
        /// <param name="exception">The exception to be logged, including its stack trace.</param>
        [DebuggerStepThrough]
        public void Error(object message, Exception exception)
        {
            _logger.Error(message, exception);
        }

        /// <summary>
        /// Logs a formatted message string with the <see cref="EasyLogLevel.Error"/> level.
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
        {
            _logger.ErrorFormat(format, arg);
        }

        /// <summary>
        /// Logs a formatted message string with the <see cref="EasyLogLevel.Error"/> level.
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
        {
            _logger.ErrorFormat(format, arg1, arg2);
        }

        /// <summary>
        /// Logs a formatted message string with the <see cref="EasyLogLevel.Error"/> level.
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
        {
            _logger.ErrorFormat(format, arg1, arg2, arg3);
        }

        /// <summary>
        /// Logs a formatted message string with the <see cref="EasyLogLevel.Error"/> level.
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
        {
            _logger.ErrorFormat(format, args);
        }

        /// <summary>
        /// Logs a formatted message string with the <see cref="EasyLogLevel.Error"/> level.
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
        {
            _logger.ErrorFormat(provider, format, args);
        }

        /// <summary>
        /// Logs a message object with the <see cref="EasyLogLevel.Fatal"/> level.
        /// </summary>
        /// <param name="message">The message object to be logged.</param>
        /// <remarks>
        /// <para>
        /// This method first checks if this logger is <c>Fatal</c> enabled by comparing the level of 
        /// this logger with the <see cref="EasyLogLevel.Fatal"/> level. If this logger is <c>Fatal</c> 
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
        public void Fatal(object message)
        {
            _logger.Fatal(message);
        }

        /// <summary>
        /// Logs a message object with the <see cref="EasyLogLevel.Fatal"/> level including 
        /// the stack trace of the <see cref="T:System.Exception"/> passed as a parameter.
        /// </summary>
        /// <param name="message">The message object to be logged.</param>
        /// <param name="exception">The exception to be logged, including its stack trace.</param>
        [DebuggerStepThrough]
        public void Fatal(object message, Exception exception)
        {
            _logger.Fatal(message, exception);
        }

        /// <summary>
        /// Logs a formatted message string with the <see cref="EasyLogLevel.Fatal"/> level.
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
        {
            _logger.FatalFormat(format, arg);
        }

        /// <summary>
        /// Logs a formatted message string with the <see cref="EasyLogLevel.Fatal"/> level.
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
        {
            _logger.FatalFormat(format, arg1, arg2);
        }

        /// <summary>
        /// Logs a formatted message string with the <see cref="EasyLogLevel.Fatal"/> level.
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
        {
            _logger.FatalFormat(format, arg1, arg2, arg3);
        }

        /// <summary>
        /// Logs a formatted message string with the <see cref="EasyLogLevel.Fatal"/> level.
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
        {
            _logger.FatalFormat(format, args);
        }

        /// <summary>
        /// Logs a formatted message string with the <see cref="EasyLogLevel.Fatal"/> level.
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
        {
            _logger.FatalFormat(provider, format, args);
        }

        /// <summary>
        /// Checks if this logger is enabled for the given <paramref name="level"/> passed as parameter. 
        /// </summary>
        [DebuggerStepThrough]
        public bool IsEnabledFor(Level level)
        {
            return _logger.Logger.IsEnabledFor(level);
        }

        /// <summary>
        /// Generates a logging event for the specified <paramref name="level"/> using 
        /// the <paramref name="message"/> and <paramref name="exception"/>. 
        /// </summary>
        [DebuggerStepThrough]
        public void Log(Level level, object message, Exception exception = null)
        {
            _logger.Logger.Log(ThisDeclaringType, level, message, exception);
        }

        /// <summary>
        /// Flag indicating whether the logger is enabled for <see cref="System.Diagnostics.Trace"/> messages.
        /// </summary>
        public bool IsTraceEnabled => _logger.Logger.IsEnabledFor(TraceLevel);

        /// <summary>
        /// Flag indicating whether the logger is enabled for <see cref="System.Diagnostics.Debug"/> messages.
        /// </summary>
        public bool IsDebugEnabled => _logger.IsDebugEnabled;

        /// <summary>
        /// Flag indicating whether the logger is enabled for <see cref="EasyLogLevel.Info"/> messages.
        /// </summary>
        public bool IsInfoEnabled => _logger.IsInfoEnabled;

        /// <summary>
        /// Flag indicating whether the logger is enabled for <see cref="EasyLogLevel.Warn"/> messages.
        /// </summary>
        public bool IsWarnEnabled => _logger.IsWarnEnabled;

        /// <summary>
        /// Flag indicating whether the logger is enabled for <see cref="EasyLogLevel.Error"/> messages.
        /// </summary>
        public bool IsErrorEnabled => _logger.IsErrorEnabled;

        /// <summary>
        /// Flag indicating whether the logger is enabled for <see cref="EasyLogLevel.Fatal"/> messages.
        /// </summary>
        public bool IsFatalEnabled => _logger.IsFatalEnabled;
    }
}
