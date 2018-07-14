namespace Easy.Logger.Tests.Unit
{
    using System;
    using System.Globalization;
    using Easy.Logger.Interfaces;
    using log4net;
    using log4net.Core;
    using Moq;
    using NUnit.Framework;
    using Shouldly;
    
    [TestFixture]
    public sealed class Log4NetLoggerTests
    {
        private Mock<ILog> _mockedLogger;
        private Mock<ILogger> _mockedInnerLogger;
        private IEasyLogger _logger;

        [OneTimeSetUp]
        public void SetUp()
        {
            _mockedInnerLogger = new Mock<ILogger>();
            _mockedInnerLogger.Setup(l => l.Name).Returns("Inner Logger");

            _mockedLogger = new Mock<ILog>();
            _mockedLogger.Setup(l => l.Logger).Returns(_mockedInnerLogger.Object);
            _mockedInnerLogger.Setup(l => l.IsEnabledFor(Level.Trace)).Returns(true);
            _mockedInnerLogger.Setup(l => l.IsEnabledFor(Level.Debug)).Returns(true);
            _mockedInnerLogger.Setup(l => l.IsEnabledFor(Level.Info)).Returns(true);
            _mockedInnerLogger.Setup(l => l.IsEnabledFor(Level.Warn)).Returns(true);
            _mockedInnerLogger.Setup(l => l.IsEnabledFor(Level.Error)).Returns(true);
            _mockedInnerLogger.Setup(l => l.IsEnabledFor(Level.Fatal)).Returns(true);

            _logger = new Log4NetLogger(_mockedLogger.Object);
            _logger.Name.ShouldBe("Inner Logger");
        }

        [Test]
        public void When_logging_using_trace()
        {
            _logger.Trace("Hi");
            _mockedInnerLogger.Verify(l => l.Log(typeof(Log4NetLogger), Level.Trace, "Hi", null), Times.Once);

            var ex = new InvalidOperationException();
            _logger.Trace("Ooops", ex);
            _mockedInnerLogger.Verify(l => l.Log(typeof(Log4NetLogger), Level.Trace, "Ooops", ex), Times.Once);
        
            _logger.TraceFormat("E:{0}", 1);
            _mockedInnerLogger.Verify(l => l.Log(typeof(Log4NetLogger), Level.Trace, "E:1", null), Times.Once);

            _logger.TraceFormat("E:{0}, {1}", 1, 2);
            _mockedInnerLogger.Verify(l => l.Log(typeof(Log4NetLogger), Level.Trace, "E:1, 2", null), Times.Once);

            _logger.TraceFormat("E:{0}, {1}, {2}", 1, 2, 3);
            _mockedInnerLogger.Verify(l => l.Log(typeof(Log4NetLogger), Level.Trace, "E:1, 2, 3", null), Times.Once);

            _logger.TraceFormat("E:{0}, {1}, {2}, {3}, {4}", 1, 2, 3, 4, 5);
            _mockedInnerLogger.Verify(l => l.Log(typeof(Log4NetLogger), Level.Trace, "E:1, 2, 3, 4, 5", null), Times.Once);

            var italianCulture = new CultureInfo("it-It");
            var date = new DateTime(2000, 12, 28, 1, 4, 43, 0);
            _logger.TraceFormat(italianCulture, "Date: {0}", date);
            _mockedInnerLogger.Verify(l => l.Log(typeof(Log4NetLogger), Level.Trace, "Date: 28/12/2000 01:04:43", null), Times.Once);
        }

        [Test]
        public void When_logging_using_debug()
        {
            _logger.Debug("Hi");
            _mockedInnerLogger.Verify(l => l.Log(typeof(Log4NetLogger), Level.Debug, "Hi", null), Times.Once);

            var ex = new InvalidOperationException();
            _logger.Debug("Ooops", ex);
            _mockedInnerLogger.Verify(l => l.Log(typeof(Log4NetLogger), Level.Debug, "Ooops", ex), Times.Once);
            
            _logger.DebugFormat("E:{0}", 1);
            _mockedInnerLogger.Verify(l => l.Log(typeof(Log4NetLogger), Level.Debug, It.IsAny<string>(), null), Times.Exactly(2));

            _logger.DebugFormat("E:{0}, {1}", 1, 2);
            _mockedInnerLogger.Verify(l => l.Log(typeof(Log4NetLogger), Level.Debug, It.IsAny<string>(), null), Times.Exactly(3));

            _logger.DebugFormat("E:{0}, {1}, {2}", 1, 2, 3);
            _mockedInnerLogger.Verify(l => l.Log(typeof(Log4NetLogger), Level.Debug, It.IsAny<string>(), null), Times.Exactly(4));

            _logger.DebugFormat("E:{0}, {1}, {2}, {3}, {4}", 1, 2, 3, 4, 5);
            _mockedInnerLogger.Verify(l => l.Log(typeof(Log4NetLogger), Level.Debug, It.IsAny<string>(), null), Times.Exactly(5));

            var italianCulture = new CultureInfo("it-It");
            var date = new DateTime(2000, 12, 28, 1, 4, 43, 0);
            _logger.DebugFormat(italianCulture, "Date: {0}", date);
            _mockedInnerLogger.Verify(l => l.Log(typeof(Log4NetLogger), Level.Debug, It.IsAny<string>(), null), Times.Exactly(6));
        }

        [Test]
        public void When_logging_using_info()
        {
            _logger.Info("Hi");
            _mockedInnerLogger.Verify(l => l.Log(typeof(Log4NetLogger), Level.Info, "Hi", null), Times.Once);

            var ex = new InvalidOperationException();
            _logger.Info("Ooops", ex);
            _mockedInnerLogger.Verify(l => l.Log(typeof(Log4NetLogger), Level.Info, "Ooops", ex), Times.Once);

            _logger.InfoFormat("E:{0}", 1);
            _mockedInnerLogger.Verify(l => l.Log(typeof(Log4NetLogger), Level.Info, It.IsAny<string>(), null), Times.Exactly(2));

            _logger.InfoFormat("E:{0}, {1}", 1, 2);
            _mockedInnerLogger.Verify(l => l.Log(typeof(Log4NetLogger), Level.Info, It.IsAny<string>(), null), Times.Exactly(3));

            _logger.InfoFormat("E:{0}, {1}, {2}", 1, 2, 3);
            _mockedInnerLogger.Verify(l => l.Log(typeof(Log4NetLogger), Level.Info, It.IsAny<string>(), null), Times.Exactly(4));

            _logger.InfoFormat("E:{0}, {1}, {2}, {3}, {4}", 1, 2, 3, 4, 5);
            _mockedInnerLogger.Verify(l => l.Log(typeof(Log4NetLogger), Level.Info, It.IsAny<string>(), null), Times.Exactly(5));

            var italianCulture = new CultureInfo("it-It");
            var date = new DateTime(2000, 12, 28, 1, 4, 43, 0);
            _logger.InfoFormat(italianCulture, "Date: {0}", date);
            _mockedInnerLogger.Verify(l => l.Log(typeof(Log4NetLogger), Level.Info, It.IsAny<string>(), null), Times.Exactly(6));
        }

        [Test]
        public void When_logging_using_warn()
        {
            _logger.Warn("Hi");
            _mockedInnerLogger.Verify(l => l.Log(typeof(Log4NetLogger), Level.Warn, "Hi", null), Times.Once);

            var ex = new InvalidOperationException();
            _logger.Warn("Ooops", ex);
            _mockedInnerLogger.Verify(l => l.Log(typeof(Log4NetLogger), Level.Warn, "Ooops", ex), Times.Once);
        
            _logger.WarnFormat("E:{0}", 1);
            _mockedInnerLogger.Verify(l => l.Log(typeof(Log4NetLogger), Level.Warn, It.IsAny<string>(), null), Times.Exactly(2));

            _logger.WarnFormat("E:{0}, {1}", 1, 2);
            _mockedInnerLogger.Verify(l => l.Log(typeof(Log4NetLogger), Level.Warn, It.IsAny<string>(), null), Times.Exactly(3));

            _logger.WarnFormat("E:{0}, {1}, {2}", 1, 2, 3);
            _mockedInnerLogger.Verify(l => l.Log(typeof(Log4NetLogger), Level.Warn, It.IsAny<string>(), null), Times.Exactly(4));

            _logger.WarnFormat("E:{0}, {1}, {2}, {3}, {4}", 1, 2, 3, 4, 5);
            _mockedInnerLogger.Verify(l => l.Log(typeof(Log4NetLogger), Level.Warn, It.IsAny<string>(), null), Times.Exactly(5));

            var italianCulture = new CultureInfo("it-It");
            var date = new DateTime(2000, 12, 28, 1, 4, 43, 0);
            _logger.WarnFormat(italianCulture, "Date: {0}", date);
            _mockedInnerLogger.Verify(l => l.Log(typeof(Log4NetLogger), Level.Warn, It.IsAny<string>(), null), Times.Exactly(6));
        }

        [Test]
        public void When_logging_using_error()
        {
            _logger.Error("Hi");
            _mockedInnerLogger.Verify(l => l.Log(typeof(Log4NetLogger), Level.Error, "Hi", null), Times.Once);

            var ex = new InvalidOperationException();
            _logger.Error("Ooops", ex);
            _mockedInnerLogger.Verify(l => l.Log(typeof(Log4NetLogger), Level.Error, "Ooops", ex), Times.Once);
        
            _logger.ErrorFormat("E:{0}", 1);
            _mockedInnerLogger.Verify(l => l.Log(typeof(Log4NetLogger), Level.Error, It.IsAny<string>(), null), Times.Exactly(2));

            _logger.ErrorFormat("E:{0}, {1}", 1, 2);
            _mockedInnerLogger.Verify(l => l.Log(typeof(Log4NetLogger), Level.Error, It.IsAny<string>(), null), Times.Exactly(3));

            _logger.ErrorFormat("E:{0}, {1}, {2}", 1, 2, 3);
            _mockedInnerLogger.Verify(l => l.Log(typeof(Log4NetLogger), Level.Error, It.IsAny<string>(), null), Times.Exactly(4));

            _logger.ErrorFormat("E:{0}, {1}, {2}, {3}, {4}", 1, 2, 3, 4, 5);
            _mockedInnerLogger.Verify(l => l.Log(typeof(Log4NetLogger), Level.Error, It.IsAny<string>(), null), Times.Exactly(5));

            var italianCulture = new CultureInfo("it-It");
            var date = new DateTime(2000, 12, 28, 1, 4, 43, 0);
            _logger.ErrorFormat(italianCulture, "Date: {0}", date);
            _mockedInnerLogger.Verify(l => l.Log(typeof(Log4NetLogger), Level.Error, It.IsAny<string>(), null), Times.Exactly(6));
        }

        [Test]
        public void When_logging_using_fatal()
        {
            _logger.Fatal("Hi");
            _mockedInnerLogger.Verify(l => l.Log(typeof(Log4NetLogger), Level.Fatal, "Hi", null), Times.Once);

            var ex = new InvalidOperationException();
            _logger.Fatal("Ooops", ex);
            _mockedInnerLogger.Verify(l => l.Log(typeof(Log4NetLogger), Level.Fatal, "Ooops", ex), Times.Once);
        
            _logger.FatalFormat("E:{0}", 1);
            _mockedInnerLogger.Verify(l => l.Log(typeof(Log4NetLogger), Level.Fatal, It.IsAny<string>(), null), Times.Exactly(2));

            _logger.FatalFormat("E:{0}, {1}", 1, 2);
            _mockedInnerLogger.Verify(l => l.Log(typeof(Log4NetLogger), Level.Fatal, It.IsAny<string>(), null), Times.Exactly(3));

            _logger.FatalFormat("E:{0}, {1}, {2}", 1, 2, 3);
            _mockedInnerLogger.Verify(l => l.Log(typeof(Log4NetLogger), Level.Fatal, It.IsAny<string>(), null), Times.Exactly(4));

            _logger.FatalFormat("E:{0}, {1}, {2}, {3}, {4}", 1, 2, 3, 4, 5);
            _mockedInnerLogger.Verify(l => l.Log(typeof(Log4NetLogger), Level.Fatal, It.IsAny<string>(), null), Times.Exactly(5));

            var italianCulture = new CultureInfo("it-It");
            var date = new DateTime(2000, 12, 28, 1, 4, 43, 0);
            _logger.FatalFormat(italianCulture, "Date: {0}", date);
            _mockedInnerLogger.Verify(l => l.Log(typeof(Log4NetLogger), Level.Fatal, It.IsAny<string>(), null), Times.Exactly(6));
        }

        [Test]
        public void When_checking_logger_levels()
        {
            _logger.IsDebugEnabled.ShouldBeFalse();
            _mockedLogger.Verify(l => l.IsDebugEnabled, Times.Once);

            _logger.IsInfoEnabled.ShouldBeFalse();
            _mockedLogger.Verify(l => l.IsInfoEnabled, Times.Once);

            _logger.IsWarnEnabled.ShouldBeFalse();
            _mockedLogger.Verify(l => l.IsWarnEnabled, Times.Once);

            _logger.IsErrorEnabled.ShouldBeFalse();
            _mockedLogger.Verify(l => l.IsErrorEnabled, Times.Once);

            _logger.IsFatalEnabled.ShouldBeFalse();
            _mockedLogger.Verify(l => l.IsFatalEnabled, Times.Once);
        }
    }
}