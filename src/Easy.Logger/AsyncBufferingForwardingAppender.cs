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
        private readonly Sequencer<Action> _sequencer;
        private readonly TimeSpan _idleTimeThreshold;
        private readonly Timer _idleFlushTimer;
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
        /// Creates an instance of the <see cref="AsyncBufferingForwardingAppender"/>
        /// </summary>
        public AsyncBufferingForwardingAppender()
        {
            _sequencer = new Sequencer<Action>(action => action());
            _sequencer.OnException += (sender, args) => LogLog.Error(GetType(), "An exception occurred while processing LogEvents.", args.Exception);

            _idleTimeThreshold = TimeSpan.FromMilliseconds(500);
            _idleFlushTimer = new Timer(state => _sequencer.TryEnqueue(InvokeFlushIfIdle), null, _idleTimeThreshold, _idleTimeThreshold);
        }

        /// <summary>
        /// Activates the options for this appender.
        /// </summary>
        public override void ActivateOptions()
        {
            base.ActivateOptions();
            LogWarningIfLossy();
        }

        /// <summary>
        /// Forwards the events to every configured appender. 
        /// </summary>
        /// <param name="events">The events that need to be forwarded</param>
        protected override void SendBuffer(LoggingEvent[] events)
        {
            if (!_sequencer.ShutdownRequested)
            {
                _sequencer.Enqueue(() =>
                {
                    base.SendBuffer(events);
                    _lastFlushTime = DateTime.UtcNow;
                });
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
            base.OnClose();
        }

        /// <summary>
        /// This only flushes if <see cref="BufferingAppenderSkeleton.Lossy"/> is <c>False</c>.
        /// </summary>
        private void InvokeFlushIfIdle()
        {
            if (!IsIdle) { return; }
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