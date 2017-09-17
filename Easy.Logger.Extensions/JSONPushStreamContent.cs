namespace Easy.Logger.Extensions
{
    using System.Diagnostics;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    /// <summary>
    /// An abstraction for pushing stream of <c>JSON</c> as content.
    /// <remarks>
    /// <see href="http://www.thomaslevesque.com/2013/11/30/uploading-data-with-httpclient-using-a-push-model/"/>.
    /// </remarks>
    /// </summary>
    internal sealed class JSONPushStreamContent : HttpContent
    {
        private static readonly Task CompletedTask;
        private static readonly JsonSerializer Serializer;
        private static readonly MediaTypeHeaderValue ContentType;

        private readonly object _payload;

        static JSONPushStreamContent()
        {
            CompletedTask = Task.FromResult(false);
            ContentType = new MediaTypeHeaderValue("application/json");
            Serializer = new JsonSerializer
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                Formatting = Formatting.None
            };

            PrettifyJSON();
        }

        /// <summary>
        /// Creates an instance of the <see cref="JSONPushStreamContent"/>.
        /// </summary>
        /// <param name="payload">The payload to serialize as content.</param>
        public JSONPushStreamContent(object payload)
        {
            _payload = payload;
            Headers.ContentType = ContentType;
        }

        protected override Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            using (var writer = new StreamWriter(stream, Encoding.UTF8, 1024, true))
            {
                Serializer.Serialize(writer, _payload);
            }

            return CompletedTask;
        }

        protected override bool TryComputeLength(out long length)
        {
            length = -1;
            return false;
        }

        [Conditional("DEBUG")]
        private static void PrettifyJSON() => Serializer.Formatting = Formatting.Indented;
    }
}