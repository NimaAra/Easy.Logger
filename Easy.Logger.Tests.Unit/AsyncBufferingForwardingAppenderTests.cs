namespace Easy.Logger.Tests.Unit
{
    using System.Collections.Generic;
    using System.Threading;
    using log4net.Appender;
    using log4net.Core;
    using Moq;
    using NUnit.Framework;
    using Shouldly;

    [TestFixture]
    public sealed class AsyncBufferingForwardingAppenderTests
    {
        [Test]
        public void When_testing_a_non_lossy_forwarder()
        {
            var forwarder = new AsyncForwardingAppender();
            forwarder.FlushThreshold.ShouldBe(300);
            forwarder.QueueSize.ShouldBe(500);
            forwarder.Lossy.ShouldBeFalse();
            forwarder.Name.ShouldBeNull();
            forwarder.Appenders.Count.ShouldBe(0);
            forwarder.Threshold.ShouldBeNull();

            var loggedEvents = new List<LoggingEvent>();
            var mockedAppender = new Mock<IAppender>();
            mockedAppender
                .Setup(s => s.DoAppend(It.IsAny<LoggingEvent>()))
                .Callback<LoggingEvent>(loggingEvent => loggedEvents.Add(loggingEvent));

            forwarder.AddAppender(mockedAppender.Object);

            forwarder.ActivateOptions();

            loggedEvents.Count.ShouldBe(0);

            forwarder.DoAppend(new LoggingEvent(new LoggingEventData()));

            loggedEvents.Count.ShouldBe(0);

            Thread.Sleep(1000);

            loggedEvents.Count.ShouldBe(1);

            forwarder.DoAppend(new[]
            {
                new LoggingEvent(new LoggingEventData()),
                new LoggingEvent(new LoggingEventData())
            });

            Thread.Sleep(1000);

            loggedEvents.Count.ShouldBe(3);

            forwarder.DoAppend(new LoggingEvent(new LoggingEventData()));
            loggedEvents.Count.ShouldBe(3);
            forwarder.Close();
            loggedEvents.Count.ShouldBe(4);
        }

        [Test]
        public void When_testing_a_lossy_forwarder()
        {
            var forwarder = new AsyncForwardingAppender {Lossy = true};
            forwarder.FlushThreshold.ShouldBe(300);
            forwarder.QueueSize.ShouldBe(500);
            forwarder.Lossy.ShouldBeTrue();
            forwarder.Name.ShouldBeNull();
            forwarder.Appenders.Count.ShouldBe(0);
            forwarder.Threshold.ShouldBeNull();

            var loggedEvents = new List<LoggingEvent>();
            var mockedAppender = new Mock<IAppender>();
            mockedAppender
                .Setup(s => s.DoAppend(It.IsAny<LoggingEvent>()))
                .Callback<LoggingEvent>(loggingEvent => loggedEvents.Add(loggingEvent));

            forwarder.AddAppender(mockedAppender.Object);

            forwarder.ActivateOptions();

            Thread.Sleep(1000);

            loggedEvents.Count.ShouldBe(1);
            loggedEvents[0].Level.ShouldBe(Level.Warn);
            loggedEvents[0].LoggerName.ShouldBe("AsyncForwardingAppender");
            loggedEvents[0].RenderedMessage
                .ShouldContain("This forwarder has been started as 'lossy' therefore log messages may be dropped.");

            forwarder.DoAppend(new LoggingEvent(new LoggingEventData()));

            loggedEvents.Count.ShouldBe(1);

            Thread.Sleep(1000);

            loggedEvents.Count.ShouldBe(2);

            forwarder.DoAppend(new[]
            {
                new LoggingEvent(new LoggingEventData()),
                new LoggingEvent(new LoggingEventData())
            });

            Thread.Sleep(1000);

            loggedEvents.Count.ShouldBe(4);

            forwarder.DoAppend(new LoggingEvent(new LoggingEventData()));
            loggedEvents.Count.ShouldBe(4);
            forwarder.Close();
            loggedEvents.Count.ShouldBe(5);
        }
    }
}