// ReSharper disable InconsistentNaming
namespace Easy.Logger.Tests.Integration.Models
{
    using System;
    using System.Text;

    public sealed class LogPayload
    {
        public string Host { get; set; }
        public int PID { get; set; }
        public string ProcessName { get; set; }
        public string Sender { get; set; }
        public DateTimeOffset TimestampUTC { get; set; }
        public int BatchCount { get; set; }
        public LogEntry[] Entries { get; set; }

        public override string ToString()
        {
            var builder = new StringBuilder();

            foreach (var e in Entries)
            {
                builder.AppendFormat("[{0:yyyy-MM-dd HH:mm:ss,fff}] [{1}] [{2}] [{3}] [{4}] [B:{5,5}] [{6,-5}] [{7,-2}] [{8}] - {9}",
                    e.DateTimeOffset,
                    Host,
                    Sender,
                    PID,
                    ProcessName,
                    BatchCount,
                    e.Level,
                    e.ThreadID,
                    e.LoggerName,
                    e.Message);

                builder.AppendLine();

                if (e.Exception != null)
                {
                    builder.Append("\t").Append(e.Exception).AppendLine();
                }
            }

            return builder.ToString();
        }
    }

    public class LogEntry
    {
        public DateTimeOffset DateTimeOffset { get; set; }
        public string LoggerName { get; set; }
        public string Level { get; set; }
        public string ThreadID { get; set; }
        public string Message { get; set; }
        public Exception Exception { get; set; }
    }
}