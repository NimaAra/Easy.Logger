namespace Easy.Logger.Tests.Unit
{
    using System;
    using System.Threading.Tasks;
    using Easy.Logger.Extensions;
    using log4net.Core;
    using NUnit.Framework;
    using Shouldly;

    [TestFixture]
    internal sealed class HTTPAppenderTests
    {
        [Test]
        public void When_creating_appender_without_name()
        {
            var appender = new HTTPAppender
            {
                Endpoint = "http://localhost:1234"
            };

            appender.Name.ShouldBeNull();
            appender.Endpoint.ShouldBe("http://localhost:1234");

            appender.ActivateOptions();

            appender.Name.ShouldNotBeNullOrWhiteSpace();
        }
        
        [Test]
        public void When_activating_appender()
        {
            var appender = new HTTPAppender
            {
                Name = "FooBar",
                Endpoint = "http://localhost:1234"
            };

            appender.Name.ShouldBe("FooBar");
            appender.Endpoint.ShouldBe("http://localhost:1234");

            appender.ActivateOptions();

            appender.Name.ShouldBe("FooBar");
        }

        [Test]
        public void When_activating_appender_with_invalid_endpoint()
        {
            var invalidEndpoints = new[]
            {
                null,
                string.Empty,
                " ",
                "ftp://localhost:1234",
                "tcp://localhost:1234"
            };

            Array.ForEach(invalidEndpoints, e =>
            {
                Should.Throw<ArgumentException>(() => new HTTPAppender
                {
                    Endpoint = e
                }.ActivateOptions())
                    .Message.ShouldBe("Invalid endpoint.");
            });
        }

        [Test]
        public async Task When_logging_events_through_appender()
        {
            var appender = new HTTPAppender
            {
                Name = "Tester",
                Endpoint = "http://localhost:1234"
            };

            appender.DoAppend(new LoggingEvent(new LoggingEventData
            {
                Message = "Foo",
                ExceptionString = "foo",
                LoggerName = "boo",
                Level = Level.Debug,
                TimeStampUtc = DateTime.UtcNow,
                ThreadName = "12"
                
            }));
            
            appender.DoAppend(new LoggingEvent(new LoggingEventData { Message = "Bar" }));
            appender.DoAppend(new LoggingEvent(new LoggingEventData { Message = "Baz" }));

            await Task.Delay(TimeSpan.FromSeconds(1));

            appender.Close();
        }
    }
}