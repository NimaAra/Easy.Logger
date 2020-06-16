using System;

namespace Easy.Logger.Tests.Unit
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using log4net.Appender;
    using log4net.Core;
    using Moq;
    using NUnit.Framework;
    using Shouldly;

    [TestFixture]
    public sealed class AsyncBufferingForwardingAppenderTests
    {
        [Test]
        public async Task When_testing_a_non_lossy_forwarder()
        {
            var forwarder = new AsyncBufferingForwardingAppender();
            forwarder.BufferSize.ShouldBe(512);
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

            await Task.Delay(1000);

            loggedEvents.Count.ShouldBe(1);

            forwarder.DoAppend(new[]
            {
                new LoggingEvent(new LoggingEventData()),
                new LoggingEvent(new LoggingEventData())
            });

            await Task.Delay(1000);

            loggedEvents.Count.ShouldBe(3);

            forwarder.DoAppend(new LoggingEvent(new LoggingEventData()));
            loggedEvents.Count.ShouldBe(3);
            forwarder.Close();
            loggedEvents.Count.ShouldBe(4);
        }

        [Test]
        public async Task When_testing_a_lossy_forwarder()
        {
            var forwarder = new AsyncBufferingForwardingAppender
            {
                Lossy = true,
                LossyEvaluator = new LevelEvaluator(Level.Error),
                Fix = FixFlags.ThreadName | FixFlags.Exception | FixFlags.Message
            };

            forwarder.BufferSize.ShouldBe(512);
            forwarder.Lossy.ShouldBeTrue();
            forwarder.Name.ShouldBeNull();
            forwarder.Appenders.Count.ShouldBe(0);
            forwarder.Threshold.ShouldBeNull();
            forwarder.LossyEvaluator.ShouldBeOfType<LevelEvaluator>();

            var loggedEvents = new List<LoggingEvent>();
            var mockedAppender = new Mock<IAppender>();
            mockedAppender
                .Setup(s => s.DoAppend(It.IsAny<LoggingEvent>()))
                .Callback<LoggingEvent>(loggingEvent => loggedEvents.Add(loggingEvent));

            forwarder.AddAppender(mockedAppender.Object);

            forwarder.ActivateOptions();

            await Task.Delay(1000);

            loggedEvents.Count.ShouldBe(1);
            loggedEvents[0].Level.ShouldBe(Level.Warn);
            loggedEvents[0].LoggerName.ShouldBe("AsyncBufferingForwardingAppender");
            loggedEvents[0].RenderedMessage
                .ShouldContain("This is a 'lossy' appender therefore log messages may be dropped.");

            var event1 = new LoggingEvent(new LoggingEventData { Level = Level.Info });
            var event2 = new LoggingEvent(new LoggingEventData { Level = Level.Debug });
            var event3 = new LoggingEvent(new LoggingEventData { Level = Level.Error });
            var event4 = new LoggingEvent(new LoggingEventData { Level = Level.Warn });
            var event5 = new LoggingEvent(new LoggingEventData { Level = Level.Fatal });

            forwarder.LossyEvaluator.IsTriggeringEvent(event1).ShouldBeFalse();
            forwarder.LossyEvaluator.IsTriggeringEvent(event2).ShouldBeFalse();
            forwarder.LossyEvaluator.IsTriggeringEvent(event3).ShouldBeTrue();
            forwarder.LossyEvaluator.IsTriggeringEvent(event4).ShouldBeFalse();
            forwarder.LossyEvaluator.IsTriggeringEvent(event5).ShouldBeTrue();

            forwarder.DoAppend(event1);

            await Task.Delay(1000);

            loggedEvents.Count.ShouldBe(1);
            loggedEvents[0].Level.ShouldBe(Level.Warn);

            forwarder.DoAppend(new[] { event2, event3, event4 });
            forwarder.Flush(true);

            await Task.Delay(1000);

            loggedEvents.Count.ShouldBe(2);
            loggedEvents[0].Level.ShouldBe(Level.Warn);
            loggedEvents[1].Level.ShouldBe(Level.Error);

            forwarder.DoAppend(event5);
            loggedEvents.Count.ShouldBe(2);

            loggedEvents[0].Level.ShouldBe(Level.Warn);
            loggedEvents[1].Level.ShouldBe(Level.Error);

            forwarder.Close();

            loggedEvents.Count.ShouldBe(3);
            loggedEvents[0].Level.ShouldBe(Level.Warn);
            loggedEvents[1].Level.ShouldBe(Level.Error);
            loggedEvents[2].Level.ShouldBe(Level.Fatal);
        }

        [Test]
        public async Task When_testing_bounded_capacity()
        {
            var forwarder = new AsyncBufferingForwardingAppender
            {
                BoundedCapacity = 10,
                Lossy = true,
                LossyEvaluator = new LevelEvaluator(Level.Error),
                Fix = FixFlags.ThreadName | FixFlags.Exception | FixFlags.Message
            };

            forwarder.BufferSize.ShouldBe(512);
            forwarder.Lossy.ShouldBeTrue();
            forwarder.Name.ShouldBeNull();
            forwarder.Appenders.Count.ShouldBe(0);
            forwarder.Threshold.ShouldBeNull();
            forwarder.LossyEvaluator.ShouldBeOfType<LevelEvaluator>();
            forwarder.BoundedCapacity.ShouldBe(10);

            var loggedEvents = new List<LoggingEvent>();
            var mockedAppender = new Mock<IAppender>();
            mockedAppender
                .Setup(s => s.DoAppend(It.IsAny<LoggingEvent>()))
                .Callback<LoggingEvent>(loggingEvent => loggedEvents.Add(loggingEvent));

            forwarder.AddAppender(mockedAppender.Object);

            forwarder.ActivateOptions();

            await Task.Delay(1000);

            loggedEvents.Count.ShouldBe(1);
            loggedEvents[0].LoggerName.ShouldBe("AsyncBufferingForwardingAppender");

            int count = 1000000 - 1;

            for (int i = 0; i < count; i++)
            {
                forwarder.DoAppend(new LoggingEvent(new LoggingEventData { Level = Level.Error }));
            }

            forwarder.GetSequencerBoundedCapacity().ShouldBe(10);

            await Task.Delay(1000);

            forwarder.Close();

            loggedEvents.Count.ShouldBe(count + 1);
        }
    }
}