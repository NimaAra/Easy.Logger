namespace Easy.Logger.Tests.Unit
{
    using Easy.Logger.Extensions;
    using log4net.Core;
    using NUnit.Framework;
    using Shouldly;

    [TestFixture]
    internal sealed class LoggingEventExtensionsTests
    {
        [Test]
        public void When_getting_logger_name()
        {
            var logEvent1 = new LoggingEvent(new LoggingEventData { LoggerName = string.Empty });
            var logEvent2 = new LoggingEvent(new LoggingEventData { LoggerName = " " });
            var logEvent3 = new LoggingEvent(new LoggingEventData { LoggerName = "." });
            var logEvent4 = new LoggingEvent(new LoggingEventData { LoggerName = ". " });
            var logEvent5 = new LoggingEvent(new LoggingEventData { LoggerName = ".HTTPAppenderTests" });
            var logEvent6 = new LoggingEvent(new LoggingEventData { LoggerName = ".HTTPAppenderTests." });
            var logEvent7 = new LoggingEvent(new LoggingEventData { LoggerName = "HTTPAppenderTests." });
            var logEvent8 = new LoggingEvent(new LoggingEventData { LoggerName = "Easy.Logger.Tests.Integration.HTTPAppenderTests" });
            var logEvent9 = new LoggingEvent(new LoggingEventData { LoggerName = "HTTPAppenderTests" });
            var logEvent10 = new LoggingEvent(new LoggingEventData { LoggerName = ",HTTPAppenderTests" });
         
            logEvent1.GetLoggerName().ShouldBe(string.Empty);
            logEvent2.GetLoggerName().ShouldBe(" ");
            logEvent3.GetLoggerName().ShouldBe(".");
            logEvent4.GetLoggerName().ShouldBe(" ");
            logEvent5.GetLoggerName().ShouldBe("HTTPAppenderTests");
            logEvent6.GetLoggerName().ShouldBe("HTTPAppenderTests.");
            logEvent7.GetLoggerName().ShouldBe("HTTPAppenderTests.");
            logEvent8.GetLoggerName().ShouldBe("HTTPAppenderTests");
            logEvent9.GetLoggerName().ShouldBe("HTTPAppenderTests");
            logEvent10.GetLoggerName().ShouldBe(",HTTPAppenderTests");
        }
    }
}