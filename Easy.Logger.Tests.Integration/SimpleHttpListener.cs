namespace Easy.Logger.Tests.Integration
{
    using System;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;

    internal sealed class SimpleHttpListener : IDisposable
    {
        private readonly HttpListener _listener;
        private int _isDisposing;

        internal EventHandler<HttpListenerContext> OnRequest;
        internal EventHandler<HttpListenerException> OnError;

        internal SimpleHttpListener(params Uri[] prefixes)
        {
            _listener = new HttpListener();

            if (prefixes == null || prefixes.Length == 0)
            {
                _listener.Prefixes.Add("http://*:80/");
            } else
            {
                Array.ForEach(prefixes, p =>
                {
                    _listener.Prefixes.Add(p.AbsoluteUri);
                });
            }
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

                OnRequest?.Invoke(this, ctx);
            }
        }
    }
}