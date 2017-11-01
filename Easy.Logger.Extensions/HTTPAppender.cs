namespace Easy.Logger.Extensions
{
    using System;
    using System.Diagnostics;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using log4net.Appender;
    using log4net.Core;

    /// <summary>
    /// An appender for <c>POSTing</c> log events to a given endpoint.
    /// </summary>
    public sealed class HTTPAppender : AppenderSkeleton
    {
        private readonly ManualResetEventSlim _waitHandle;
        private readonly HttpClient _client;
        private readonly ThreadLocal<LoggingEvent[]> _singleLogEventPool;
        private readonly string _pid;
        private readonly string _pName;

        private int _counter;
        private string _host;

        /// <summary>
        /// Creates an instance of the <see cref="HTTPAppender"/>.
        /// </summary>
        public HTTPAppender()
        {
            _waitHandle = new ManualResetEventSlim();
            _client = new HttpClient();
            _singleLogEventPool = new ThreadLocal<LoggingEvent[]>(() => new LoggingEvent[1]);

            using (var p = Process.GetCurrentProcess())
            {
                _pid = p.Id.ToString();
                _pName = p.ProcessName;
            }
        }

        /// <summary>
        /// Gets the endpoint to which log events are <c>POSTed</c> to.
        /// </summary>
        public string Endpoint { get; set; }

        /// <summary>
        /// Gets the flag indicating whether the host name should be included in the payload.
        /// </summary>
        public bool IncludeHost { get; set; }

        /// <summary>
        /// Activates the appender options.
        /// </summary>
        public override void ActivateOptions()
        {
            base.ActivateOptions();

            if (string.IsNullOrWhiteSpace(Name))
            {
                Name = Guid.NewGuid().ToString();
            }

            if (string.IsNullOrWhiteSpace(Endpoint)
                || !Endpoint.StartsWith("HTTP", StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException("Invalid endpoint.");
            }

            _host = IncludeHost ? Dns.GetHostName() : null;
        }

        /// <summary>
        /// Serializes and <c>POST</c>s the given <paramref name="logEvent"/> to <see cref="Endpoint"/>.
        /// </summary>
        protected override void Append(LoggingEvent logEvent)
        {
            var pool = _singleLogEventPool.Value;
            pool[0] = logEvent;
            Append(pool);
        }

        /// <summary>
        /// Serializes and <c>POST</c>s the given <paramref name="logEvents"/> to <see cref="Endpoint"/>.
        /// </summary>
        protected override void Append(LoggingEvent[] logEvents) => Post(logEvents);

        /// <summary>
        /// Ensures that all pending logging events are flushed out before exiting.
        /// </summary>
        protected override void OnClose()
        {
            _waitHandle.Wait(TimeSpan.FromSeconds(1));
            _waitHandle.Dispose();
            _client.Dispose();

            base.OnClose();
        }

        private async void Post(LoggingEvent[] logEvents)
        {
            var payload = new
            {
                PID = _pid,
                ProcessName = _pName,
                Host = _host,
                Sender = Name,
                TimestampUTC = DateTimeOffset.UtcNow,
                BatchCount = Interlocked.Increment(ref _counter),
                Entries = GetEntries(logEvents)
            };
            
            _waitHandle.Reset();

            try
            {
                if (!await DoPost(payload).ConfigureAwait(false))
                {
                    // Try once more
                    await DoPost(payload).ConfigureAwait(false);
                }
            } finally
            {
                _waitHandle.Set();
            }
        }

        private async Task<bool> DoPost(object payload)
        {
            using (var content = new JSONPushStreamContent(payload))
            {
                var response = await _client.PostAsync(Endpoint, content).ConfigureAwait(false);
                return response.IsSuccessStatusCode;
            }
        }

        // ReSharper disable once SuggestBaseTypeForParameter
        private static object[] GetEntries(LoggingEvent[] logEvents)
        {
            var result = new object[logEvents.Length];
            for (var i = 0; i < logEvents.Length; i++)
            {
                var curr = logEvents[i];
                result[i] = new
                {
                    DateTimeOffset = new DateTimeOffset(curr.TimeStamp),
                    curr.LoggerName,
                    Level = curr.Level?.DisplayName,
                    ThreadID = curr.ThreadName,
                    Message = curr.RenderedMessage,
                    Exception = curr.ExceptionObject
                };
            }
            return result;
        }
    }
}