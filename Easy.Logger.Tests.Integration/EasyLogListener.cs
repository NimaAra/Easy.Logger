namespace Easy.Logger.Tests.Integration
{
    using System;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Text.RegularExpressions;
    using Easy.Logger.Tests.Integration.Models;
    using System.Threading;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    /// <summary>
    /// A simple <c>HTTP</c> listener for receiving <see cref="LogPayload"/>s.
    /// </summary>
    public sealed class EasyLogListener : IDisposable
    {
        private static readonly byte[] InvalidRequestMethodMessage = Encoding.UTF8.GetBytes("You can only POST a valid JSON payload to this server.");
        private static readonly byte[] InvalidPayloadMessage = Encoding.UTF8.GetBytes("Invalid payload. Payload should be a valid JSON.");

        private static readonly Regex JSONMediaTypeRegex =
            new Regex(@"([aA][pP][pP][lL][iI][cC][aA][tT][iI][oO][nN]/[jJ][sS][oO][nN]|[tT][eE][xX][tT]/[jJ][sS][oO][nN]|[aA][pP][pP][lL][iI][cC][aA][tT][iI][oO][nN]/[vV][nN][dD]\.\S*\+[jJ][sS][oO][nN])", RegexOptions.Compiled);

        private readonly HttpListener _listener;
        private readonly JsonSerializer _serializer;
        private int _isDisposing;

        /// <summary>
        /// Invoked when a new <see cref="LogPayload"/> is <c>POST</c>ed.
        /// </summary>
        public event EventHandler<LogPayload> OnPayload;
        
        /// <summary>
        /// Invoked when there is an error during the deserialization of <see cref="LogPayload"/>.
        /// </summary>
        public event EventHandler<Exception> OnError;

        /// <summary>
        /// Creates an instance of <see cref="EasyLogListener"/>.
        /// </summary>
        /// <param name="prefix">The prefix the listener will be listening on.</param>
        public EasyLogListener(Uri prefix)
        {
            _listener = new HttpListener();
            _listener.Prefixes.Add(prefix.AbsoluteUri);

            _serializer = new JsonSerializer
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                Formatting = Formatting.None
            };
        }

        /// <summary>
        /// Starts listening for payloads.
        /// </summary>
        public void Start() => StartListening();

        /// <summary>
        /// Stops and releases resources used by the instance.
        /// </summary>
        public void Dispose()
        {
            Interlocked.Increment(ref _isDisposing);

            _listener.Stop();
            _listener.Close();
        }

        private async void StartListening()
        {
            _listener.Start();
            while (_listener.IsListening)
            {
                HttpListenerContext ctx = null;

                try
                {
                    ctx = await _listener.GetContextAsync().ConfigureAwait(false);
                } catch (HttpListenerException e) { HandleException(e); }

                if (ctx == null) { return; }

                var req = ctx.Request;
                var resp = ctx.Response;

                if (IsValidRequest(req))
                {
                    try
                    {
                        var payload = Deserialize(req.InputStream);
                        OnPayload?.Invoke(this, payload);
                        resp.StatusCode = (int) HttpStatusCode.Accepted;
                    } 
                    catch (HttpListenerException) { continue; }
                    catch (Exception e)
                    {
                        HandleException(e);
                        resp.StatusCode = (int)HttpStatusCode.BadRequest;
                        resp.OutputStream.Write(InvalidPayloadMessage, 0, InvalidPayloadMessage.Length);
                    }
                } else
                {
                    resp.StatusCode = (int)HttpStatusCode.NotAcceptable;
                    resp.OutputStream.Write(InvalidRequestMethodMessage, 0, InvalidRequestMethodMessage.Length);
                }

                resp.Close();
            }
        }

        private LogPayload Deserialize(Stream stream)
        {
            using (var sr = new StreamReader(stream, Encoding.UTF8, true, 1024, true))
            using (var jsonTextReader = new JsonTextReader(sr) { CloseInput = false })
            {
                return _serializer.Deserialize<LogPayload>(jsonTextReader);
            }
        }

        private void HandleException(Exception e)
        {
            // If we are NOT disposing, then we should report exception
            if (Interlocked.CompareExchange(ref _isDisposing, 0, 0) == 0)
            {
                OnError?.Invoke(this, e);
            }
        }

        private static bool IsValidRequest(HttpListenerRequest req)
        {
            return req.HttpMethod.Equals("POST", StringComparison.Ordinal) 
                && IsJSONMediaType(req.ContentType);
        }

        private static bool IsJSONMediaType(string contentType) => 
            !string.IsNullOrEmpty(contentType) && JSONMediaTypeRegex.IsMatch(contentType);
    }
}