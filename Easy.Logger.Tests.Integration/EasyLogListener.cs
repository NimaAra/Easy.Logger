namespace Easy.Logger.Tests.Integration
{
    using System;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Threading.Tasks;
    using Easy.Logger.Tests.Integration.Models;
    using Exception = System.Exception;

    internal sealed class EasyLogListener : IDisposable
    {
        private static readonly byte[] InvalidRequestMethodMessage = Encoding.UTF8.GetBytes("You can only POST a valid JSON payload to this server.");
        private static readonly byte[] InvalidPayloadMessage = Encoding.UTF8.GetBytes("Invalid payload. Payload should be a valid JSON.");

        private static readonly Regex JSONMediaTypeRegex =
            new Regex(@"([aA][pP][pP][lL][iI][cC][aA][tT][iI][oO][nN]/[jJ][sS][oO][nN]|[tT][eE][xX][tT]/[jJ][sS][oO][nN]|[aA][pP][pP][lL][iI][cC][aA][tT][iI][oO][nN]/[vV][nN][dD]\.\S*\+[jJ][sS][oO][nN])", RegexOptions.Compiled);

        private readonly Func<Stream, LogPayload> _deserializer;
        private readonly HttpListener _listener;
        private int _isDisposing;

        public EventHandler<LogPayload> OnPayload;
        public EventHandler<Exception> OnError;

        internal EasyLogListener(Uri prefix, Func<Stream, LogPayload> deserliazer)
        {
            _listener = new HttpListener();
            _listener.Prefixes.Add(prefix.AbsoluteUri);
            _deserializer = deserliazer;
        }

        internal Task ListenAsync()
        {
            ListenAsyncImpl();
            return Task.FromResult(false);
        }

        public void Dispose()
        {
            Interlocked.Increment(ref _isDisposing);

            _listener.Stop();
            _listener.Close();
        }

        private async void ListenAsyncImpl()
        {
            _listener.Start();
            while (_listener.IsListening)
            {
                HttpListenerContext ctx = null;

                try
                {
                    ctx = await _listener.GetContextAsync();
                } catch (HttpListenerException e)
                {
                    // If we are NOT disposing, then we should report exception
                    if (Interlocked.CompareExchange(ref _isDisposing, 0, 0) == 0)
                    {
                        OnError?.Invoke(this, e);
                    }
                }

                if (ctx == null) { return; }

                var req = ctx.Request;
                var resp = ctx.Response;

                if (IsValidRequest(req))
                {
                    try
                    {
                        var payload = _deserializer(req.InputStream);
                        OnPayload?.Invoke(this, payload);
                        resp.StatusCode = (int)HttpStatusCode.Accepted;
                    } catch (Exception e)
                    {
                        OnError?.Invoke(this, e);
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

        private static bool IsValidRequest(HttpListenerRequest req)
        {
            return req.HttpMethod.Equals("POST", StringComparison.Ordinal) 
                && IsJSONMediaType(req.ContentType);
        }

        private static bool IsJSONMediaType(string contentType) => 
            !string.IsNullOrEmpty(contentType) && JSONMediaTypeRegex.IsMatch(contentType);
    }
}