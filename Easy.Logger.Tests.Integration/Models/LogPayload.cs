// ReSharper disable InconsistentNaming
namespace Easy.Logger.Tests.Integration.Models
{
    using System;

    public sealed class LogPayload
    {
        public string Host { get; set; }
        public int PID { get; set; }
        public string ProcessName { get; set; }
        public string Sender { get; set; }
        public DateTimeOffset TimestampUTC { get; set; }
        public int BatchCount { get; set; }
        public LogEntry[] Entries { get; set; }
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

    public class Exception
    {
        public string ClassName { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
        public object InnerException { get; set; }
        public object HelpURL { get; set; }
        public object StackTraceString { get; set; }
        public object RemoteStackTraceString { get; set; }
        public int RemoteStackIndex { get; set; }
        public object ExceptionMethod { get; set; }
        public int HResult { get; set; }
        public object Source { get; set; }
        public object WatsonBuckets { get; set; }
        public string ParamName { get; set; }
    }
}