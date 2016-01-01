namespace EasyLogger
{
    using System;
    using System.Threading;
    using log4net.Appender;
    using log4net.Core;
    using log4net.Util;
    using Timer = System.Timers.Timer;

    /// <summary>
    /// This asynchronous forwarder uses the following fix flags:
    /// FixFlags.ThreadName | FixFlags.Message | FixFlags.Exception;
    /// </summary>
    public sealed class AsyncForwardingAppender : ForwardingAppender
    {
        private readonly object _locker = new object();
        private LoggingEvent[] _flushBuffer;
        private int _flushBufferIndex;
        private Sequencer<LoggingEvent> _backgroundQueue;
        private Timer _idleFlushTimer;
        private DateTime _lastFlushTime;
        private TimeSpan _idleTimeThreshold = TimeSpan.FromSeconds(1);
        private const string IdleFlushFlag = "Idle-2B80D553-5676-4B9B-884B-11D500CBA877";
        private const string ClosingFlushFlag = "Closing-BBD3D431-3886-4221-8843-0211F2EAB2EF";

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

        public override void ActivateOptions()
        {
            lock (_locker)
            {
                _flushBuffer = new LoggingEvent[FlushThreshold];
                _backgroundQueue = new Sequencer<LoggingEvent>(LogImpl, QueueSize);
                _backgroundQueue.OnException += BackgroundQueueOnException;

                _idleFlushTimer = new Timer(_idleTimeThreshold.TotalSeconds * 1000);
                _idleFlushTimer.Elapsed += (sender, args) =>
                {
                    SignalManualFlush(IdleFlushFlag);
                };
                
                base.ActivateOptions();

                if (Lossy) { LogLossyWarning(); }
                _idleFlushTimer.Start();
            }
        }

        protected override void Append(LoggingEvent[] loggingEvents)
        {
            Array.ForEach(loggingEvents, Append);
        }

        protected override void Append(LoggingEvent loggingEvent)
        {
            if (Lossy && _backgroundQueue.PendingCount == QueueSize) { return; }

            loggingEvent.Fix = FixFlags.ThreadName | FixFlags.Message | FixFlags.Exception;
            _backgroundQueue.Enqueue(loggingEvent);
        }

        protected override void OnClose()
        {
            lock (_locker)
            {
                _idleFlushTimer.Dispose();

                SignalManualFlush(ClosingFlushFlag);

                _backgroundQueue.Shutdown();
                _backgroundQueue.OnException -= BackgroundQueueOnException;

                base.OnClose();
            }
        }

        private void LogImpl(LoggingEvent logEvent)
        {
            FlushType type;
            if (IsManualFlush(logEvent, out type))
            {
                if (type == FlushType.Idle)
                {
                    if (IsIdle) { DoFlush(true); }
                }
                else if (type == FlushType.Closing)
                {
                    DoFlush(true);

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

        private static bool IsManualFlush(LoggingEvent logEvent, out FlushType type)
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

        private void SignalManualFlush(string flag)
        {
            var logEvent = new LoggingEvent(new LoggingEventData
            {
                Level = Level.Notice,
                LoggerName = flag
            });
            _backgroundQueue.Enqueue(logEvent);
        }

        private void BackgroundQueueOnException(object sender, SequencerEventArgs args)
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