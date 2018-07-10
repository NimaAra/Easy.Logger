namespace Easy.Logger.Extensions
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using log4net.Appender;
    using log4net.Core;
#if NETSTANDARD1_3
    using System.Text;
    using Newtonsoft.Json;
    using JsonSerializer = Newtonsoft.Json.JsonSerializer;
    using Newtonsoft.Json.Serialization;    
#else
    using Utf8Json;
    using Utf8Json.Formatters;
    using Utf8Json.Resolvers;
#endif
    
    /// <summary>
    /// An appender for <c>POSTing</c> log events to a given endpoint.
    /// </summary>
    public sealed class HTTPAppender : AppenderSkeleton
    {
#if NETSTANDARD1_3
        private static readonly JsonSerializer Serializer;
#endif
        private readonly ThreadLocal<LoggingEvent[]> _singleLogEventPool;
        private readonly int _pid;
        private readonly string _processName;
        
        private int _counter;
        private string _host, _sender;

        static HTTPAppender()
        {
#if NETSTANDARD1_3
            Serializer = new JsonSerializer
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                Formatting = Formatting.None
            };
#endif
        }

        /// <summary>
        /// Creates an instance of the <see cref="HTTPAppender"/>.
        /// </summary>
        public HTTPAppender()
        {
            _singleLogEventPool = new ThreadLocal<LoggingEvent[]>(() => new LoggingEvent[1]);

            using (var p = Process.GetCurrentProcess())
            {
                _pid = p.Id;
                _processName = p.ProcessName;
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
            _sender = Name;
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

        private async void Post(LoggingEvent[] logEvents)
        {
            var payload = new Payload
            {
                PID = _pid,
                ProcessName = _processName,
                Host = _host,
                Sender = _sender,
                TimestampUTC = DateTimeOffset.UtcNow,
                BatchNo = Interlocked.Increment(ref _counter),
                Entries = GetEntries(logEvents)
            };

            if (!await DoPost(payload).ConfigureAwait(false))
            {
                // Try once more
                await DoPost(payload).ConfigureAwait(false);
            }
        }

        private async Task<bool> DoPost(Payload payload)
        {
            var req = (HttpWebRequest)WebRequest.Create(Endpoint);

            /* ToDo - Consider setting the proxy
             * string proxyUrl = "proxy.myproxy.com";
             * int proxyPort = 8080;
             * WebProxy myProxy = new WebProxy(proxyUrl, proxyPort);
             *
             * HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(Endpoint);
             * req.Proxy = myProxy;
             */
            req.Proxy = null;
            req.Method = "POST";
            req.ContentType = "application/json";

            try
            {
                using (var stream = await req.GetRequestStreamAsync().ConfigureAwait(false))
                {
#if NETSTANDARD1_3
                    using (TextWriter writer = new StreamWriter(stream, Encoding.UTF8, 1024, true))
                    using (JsonTextWriter jsonWriter = new JsonTextWriter(writer))
                    {
                        Serializer.Serialize(jsonWriter, payload);
                    }
#else
                    JsonSerializer.Serialize(stream, payload, StandardResolver.AllowPrivateCamelCase);
#endif
                    using (var resp = (HttpWebResponse) await req.GetResponseAsync().ConfigureAwait(false))
                    {
                        return IsSuccessStatusCode(resp);
                    }
                }
            } catch (WebException)
            {
                return false;
            }
        }

        private static Entry[] GetEntries(LoggingEvent[] logEvents)
        {
            var result = new Entry[logEvents.Length];
            for (var i = 0; i < logEvents.Length; i++)
            {
                var curr = logEvents[i];

                result[i] = new Entry
                {
                    DateTimeOffset = new DateTimeOffset(curr.TimeStamp),
                    LoggerName = curr.LoggerName,
                    Level = curr.Level?.DisplayName,
                    ThreadID = curr.ThreadName,
                    Message = curr.RenderedMessage,
                    Exception = curr.ExceptionObject
                };
            }
            return result;
        }

        private static bool IsSuccessStatusCode(HttpWebResponse response)
        {
            if (response.StatusCode >= HttpStatusCode.OK)
            {
                return response.StatusCode <= (HttpStatusCode)299;
            }
            return false;
        }

        [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Local")]
        private sealed class Payload
        {
            // ReSharper disable once InconsistentNaming
            public int PID { get; set; }
            public string ProcessName { get; set; }
            public string Host { get; set; }
            public string Sender { get; set; }
            public DateTimeOffset TimestampUTC { get; set; }
            public int BatchNo { get; set; }
            public Entry[] Entries { get; set; }
        }

        [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Local")]
        private struct Entry
        {
            public DateTimeOffset DateTimeOffset { get; set; }
            public string LoggerName { get; set; }
            public string Level { get; set; }
            // ReSharper disable once InconsistentNaming
            public string ThreadID { get; set; }
            public string Message { get; set; }
            public Exception Exception { get; set; }
        }
    }
}