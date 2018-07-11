namespace Easy.Logger
{
    using System;
    using System.Threading;
    using log4net.Appender;
    using log4net.Core;
    using log4net.Util;

    /// <summary>
    /// An appender which batches the log events and asynchronously forwards them to any configured appenders.
    /// <seealso href="www.nimaara.com/2016/01/01/high-performance-logging-log4net/"/>
    /// </summary>
    public sealed class AsyncBufferingForwardingAppender : BufferingForwardingAppender
    {
        private const int DEFAULT_IDLE_TIME = 500;

        private readonly Sequencer<LoggingEvent[]> _sequencer;

        private TimeSpan _idleTimeThreshold;
        private Timer _idleFlushTimer;
        private DateTime _lastFlushTime;

        private bool IsIdle
        {
            get
            {
                if (DateTime.UtcNow - _lastFlushTime >= _idleTimeThreshold) { return true; }
                return false;
            }
        }

        /// <summary>
        /// Gets or sets the idle-time in milliseconds at which any pending logging events are flushed.
        /// <value>The idle-time in milliseconds.</value>
        /// <remarks>
        /// <para>
        /// The value should be a positive integer representing the maximum idle-time of logging events 
        /// to be collected in the <see cref="AsyncBufferingForwardingAppender"/>. When this value is 
        /// reached, buffered events are then flushed. By default the idle-time is <c>500</c> milliseconds.
        /// </para>
        /// <para>
        /// If the <see cref="IdleTime"/> is set to a value less than or equal to <c>0</c>
        /// then use the default value is used.
        /// </para>
        /// </remarks>
        /// </summary>
        public int IdleTime { get; set; } = DEFAULT_IDLE_TIME;

        /// <summary>
        /// Creates an instance of the <see cref="AsyncBufferingForwardingAppender"/>
        /// </summary>
        public AsyncBufferingForwardingAppender()
        {
            _sequencer = new Sequencer<LoggingEvent[]>(Process);
            _sequencer.OnException += (sender, args) 
                => LogLog.Error(GetType(), "An exception occurred while processing LogEvents.", args.Exception);
        }

        /// <summary>
        /// Activates the options for this appender.
        /// </summary>
        public override void ActivateOptions()
        {
            base.ActivateOptions();

            LogWarningIfLossy();

            if (IdleTime <= 0) { IdleTime = DEFAULT_IDLE_TIME; }
            
            _idleTimeThreshold = TimeSpan.FromMilliseconds(IdleTime);
            _idleFlushTimer = new Timer(InvokeFlushIfIdle, null, _idleTimeThreshold, _idleTimeThreshold);
        }

        /// <summary>
        /// Forwards the events to every configured appender. 
        /// </summary>
        /// <param name="events">The events that need to be forwarded</param>
        protected override void SendBuffer(LoggingEvent[] events)
        {
            if (!_sequencer.ShutdownRequested)
            {
                _sequencer.Enqueue(events);
            } else
            {
                base.SendBuffer(events);
            }
        }

        /// <summary>
        /// Ensures that all pending logging events are flushed out before exiting.
        /// </summary>
        protected override void OnClose()
        {
            _idleFlushTimer.Dispose();
            _sequencer.Shutdown();

            Flush();

            base.OnClose();
        }

        private void Process(LoggingEvent[] logEvents)
        {
            base.SendBuffer(logEvents);
            _lastFlushTime = DateTime.UtcNow;
        }

        /// <summary>
        /// This only flushes if <see cref="BufferingAppenderSkeleton.Lossy"/> is <c>False</c>.
        /// </summary>
        private void InvokeFlushIfIdle(object _)
        {
            if (!IsIdle) { return; }
            if (_sequencer.ShutdownRequested) { return; }

            Flush();
        }

        private void LogWarningIfLossy()
        {
            if (!Lossy) { return; }

            var warning = new LoggingEvent(new LoggingEventData
            {
                Level = Level.Warn,
                LoggerName = GetType().Name,
                ThreadName = Thread.CurrentThread.ManagedThreadId.ToString(),
                TimeStampUtc = DateTime.UtcNow,
                Message = "This is a 'lossy' appender therefore log messages may be dropped."
            });

            Lossy = false;
            Append(warning);
            Flush();
            Lossy = true;
        }
    }
}