// ReSharper disable InconsistentNaming
namespace Easy.Logger.Tests.Integration.Models
{
    using System;
    using System.Text;

    /// <summary>
    /// Represents a log payload.
    /// </summary>
    public sealed class LogPayload
    {
        /// <summary>
        /// Gets or sets the host.
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// Gets or sets the process id.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public int PID { get; set; }

        /// <summary>
        /// Gets or sets the process name.
        /// </summary>
        public string ProcessName { get; set; }

        /// <summary>
        /// Gets or sets the sender name.
        /// </summary>
        public string Sender { get; set; }

        /// <summary>
        /// Gets or sets the time stamp at which the payload was generated.
        /// </summary>
        public DateTimeOffset TimestampUTC { get; set; }

        /// <summary>
        /// Gets or sets the batch number to which the payload belong.
        /// </summary>
        public int BatchNo { get; set; }

        /// <summary>
        /// Gets or sets the entries contained in the payload.
        /// </summary>
        public LogEntry[] Entries { get; set; }

        /// <summary>
        /// Returns a textual representation of the payload.
        /// </summary>
        /// <returns></returns>
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
                    BatchNo,
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

    /// <summary>
    /// Represents a log entry.
    /// </summary>
    public class LogEntry
    {
        /// <summary>
        /// Gets or sets the date and time the entry was generated.
        /// </summary>
        public DateTimeOffset DateTimeOffset { get; set; }

        /// <summary>
        /// Gets or sets the logger name.
        /// </summary>
        public string LoggerName { get; set; }

        /// <summary>
        /// Gets or sets the level.
        /// </summary>
        public string Level { get; set; }

        /// <summary>
        /// Gets or sets the thread ID.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public string ThreadID { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the exception if any.
        /// </summary>
        public Exception Exception { get; set; }
    }
}