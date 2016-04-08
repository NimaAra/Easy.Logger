namespace Easy.Logger
{
    using System;
    using System.Threading;
    using System.Timers;
    using log4net.Appender;
    using log4net.Core;
    using log4net.Util;
    using Timer = System.Timers.Timer;

    /// <summary>
    /// An appender which batches the log events and asynchronously forwards them to the configured appenders.
    /// <seealso href="www.nimaara.com/2016/01/01/high-performance-logging-log4net/"/>
    /// <remarks>
    /// This asynchronous forwarder uses the following fix flags:
    /// FixFlags.ThreadName | FixFlags.Message | FixFlags.Exception;
    /// </remarks>
    /// </summary>
    public sealed class AsyncForwardingAppender : ForwardingAppender
    {
        private const string IdleFlushFlag = "Idle-2B80D553-5676-4B9B-884B-11D500CBA877";
        private const string ClosingFlushFlag = "Closing-BBD3D431-3886-4221-8843-0211F2EAB2EF";
        private readonly TimeSpan _idleTimeThreshold;
        private readonly LoggingEvent[] _flushBuffer;
        private readonly Sequencer<LoggingEvent> _backgroundQueue;
        private readonly Timer _idleFlushTimer;

        private int _flushBufferIndex;
        private DateTime _lastFlushTime;

        /// <summary>
        /// Specifies the queue size threshold at which any new items will
        /// be blocked until there is more room in the queue.
        /// </summary>
        public int QueueSize { get; set; } = 500;

        /// <summary>
        /// Specifies the threshold as the buffer/batch size (no of log events entries) 
        /// before being flushed out.
        /// </summary>
        public int FlushThreshold { get; set; } = 300;

        /// <summary>
        /// Specifies whether new log events should be dropped if the 
        /// <see cref="_backgroundQueue"/> is full instead of blocking/waiting the thread.
        /// </summary>
        public bool Lossy { get; set; } = false;

        private bool IsIdle
        {
            get
            {
                if (DateTime.UtcNow - _lastFlushTime >= _idleTimeThreshold) { return true; }
                return false;
            }
        }

        /// <summary>
        /// Creates an instance of the <see cref="AsyncForwardingAppender"/>
        /// </summary>
        public AsyncForwardingAppender()
        {
            _flushBuffer = new LoggingEvent[FlushThreshold];
            _backgroundQueue = new Sequencer<LoggingEvent>(LogImpl, QueueSize);
            _backgroundQueue.OnException += BackgroundQueueOnException;

            _idleTimeThreshold = TimeSpan.FromMilliseconds(500);
            _idleFlushTimer = new Timer(_idleTimeThreshold.TotalSeconds * 1000);
            _idleFlushTimer.Elapsed += InvokeFlushIfIdle;
            _idleFlushTimer.Start();
        }

        /// <summary>
        /// Activates the options for this appender.
        /// </summary>
        public override void ActivateOptions()
        {
            base.ActivateOptions();
            if (Lossy) { LogLossyWarning(); }
        }

        /// <summary>
        /// Appends an array of <see cref="LoggingEvent"/>
        /// </summary>
        /// <param name="loggingEvents">The log events to append</param>
        protected override void Append(LoggingEvent[] loggingEvents)
        {
            Array.ForEach(loggingEvents, Append);
        }

        /// <summary>
        /// Appends a single <see cref="LoggingEvent"/>
        /// </summary>
        /// <param name="loggingEvent">The log event to append</param>
        protected override void Append(LoggingEvent loggingEvent)
        {
            if (Lossy && _backgroundQueue.PendingCount == QueueSize) { return; }

            loggingEvent.Fix = FixFlags.ThreadName | FixFlags.Message | FixFlags.Exception;
            _backgroundQueue.Enqueue(loggingEvent);
        }

        /// <summary>
        /// Ensures that all pending logging events are flushed out before exiting
        /// </summary>
        protected override void OnClose()
        {
            _idleFlushTimer.Elapsed -= InvokeFlushIfIdle;
            _idleFlushTimer.Dispose();

            SignalManualFlush(ClosingFlushFlag);

            _backgroundQueue.Shutdown();
            _backgroundQueue.OnException -= BackgroundQueueOnException;

            base.OnClose();
        }

        private void LogImpl(LoggingEvent logEvent)
        {
            FlushType type;
            if (IsPreMatureFlush(logEvent, out type))
            {
                DoFlush(true);

                if (type == FlushType.Closing)
                {
                    // Flush any pending logging events before this signal
                    var pendingEvents = _backgroundQueue.PendingItems;
                    if (pendingEvents.Length > 0) { base.Append(pendingEvents); }
                }
                return;
            }

            if (_flushBufferIndex == FlushThreshold) { DoFlush(false); }

            _flushBuffer[_flushBufferIndex++] = logEvent;
        }

        private void LogLossyWarning()
        {
            var warning = new LoggingEvent(new LoggingEventData
            {
                Level = Level.Warn,
                LoggerName = GetType().Name,
                ThreadName = Thread.CurrentThread.ManagedThreadId.ToString(),
                TimeStamp = DateTime.UtcNow,
                Message = "This forwarder has been started as 'lossy' therefore log messages may be dropped."
            });

            Append(warning);
        }

        private static bool IsPreMatureFlush(LoggingEvent logEvent, out FlushType type)
        {
            if (logEvent.Level != Level.Notice)
            {
                type = FlushType.Invalid;
                return false;
            }

            if (string.CompareOrdinal(logEvent.LoggerName, IdleFlushFlag) == 0)
            {
                type = FlushType.Idle;
                return true;
            }

            if (string.CompareOrdinal(logEvent.LoggerName, ClosingFlushFlag) == 0)
            {
                type = FlushType.Closing;
                return true;
            }

            type = FlushType.Invalid;
            return false;
        }

        private void DoFlush(bool isPartialFlush)
        {
            if (_flushBufferIndex == 0) { return; }

            LoggingEvent[] eventsToAppend;

            // Partial flush due to idle detection or closing
            if (isPartialFlush)
            {
                eventsToAppend = GetSubArray(_flushBuffer, 0, _flushBufferIndex);
            }
            else
            {
                eventsToAppend = _flushBuffer;
            }

            base.Append(eventsToAppend);

            _lastFlushTime = DateTime.UtcNow;

            Array.Clear(_flushBuffer, 0, _flushBuffer.Length);
            _flushBufferIndex = 0;
        }

        private void InvokeFlushIfIdle(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            if (!IsIdle) { return; }
            SignalManualFlush(IdleFlushFlag);
        }

        private void SignalManualFlush(string flag)
        {
            var logEvent = new LoggingEvent(new LoggingEventData
            {
                Level = Level.Notice,
                LoggerName = flag
            });
            _backgroundQueue.Enqueue(logEvent);
        }

        private void BackgroundQueueOnException(object sender, SequencerExceptionEventArgs args)
        {
            LogLog.Error(GetType(), "An exception occurred while processing LogEvents.", args.Exception);
        }

        private static T[] GetSubArray<T>(T[] source, int startIndex, int length)
        {
            var result = new T[length];
            Array.Copy(source, startIndex, result, 0, length);
            return result;
        }

        private enum FlushType
        {
            Invalid = 0,
            Idle,
            Closing
        }
    }
}