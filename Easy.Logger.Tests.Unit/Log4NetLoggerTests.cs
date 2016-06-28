namespace Easy.Logger.Tests.Unit
{
    using System;
    using System.Globalization;
    using log4net;
    using Moq;
    using NUnit.Framework;
    using Shouldly;

    [TestFixture]
    public sealed class Log4NetLoggerTests
    {
        private Mock<ILog> _mockedLogger;
        private Log4NetLogger _logger;

        [OneTimeSetUp]
        public void SetUp()
        {
            _mockedLogger = new Mock<ILog>();
            _logger = new Log4NetLogger(_mockedLogger.Object);
        }

        [Test]
        public void When_logging_using_debug()
        {
            _logger.Debug("Hi");
            _mockedLogger.Verify(l => l.Debug("Hi"), Times.Once);

            var ex = new InvalidOperationException();
            _logger.Debug("Ooops", ex);
            _mockedLogger.Verify(l => l.Debug("Ooops", ex), Times.Once);
        }

        [Test]
        public void When_logging_using_debug_format()
        {
            _logger.DebugFormat("E:{0}", 1);
            _mockedLogger.Verify(l => l.DebugFormat("E:{0}", 1), Times.Once);

            _logger.DebugFormat("E:{0}, {1}", 1, 2);
            _mockedLogger.Verify(l => l.DebugFormat("E:{0}, {1}", 1, 2), Times.Once);

            _logger.DebugFormat("E:{0}, {1}, {2}", 1, 2, 3);
            _mockedLogger.Verify(l => l.DebugFormat("E:{0}, {1}, {2}", 1, 2, 3), Times.Once);

            _logger.DebugFormat("E:{0}, {1}, {2}, {3}, {4}", 1, 2, 3, 4, 5);
            _mockedLogger.Verify(l => l.DebugFormat("E:{0}, {1}, {2}, {3}, {4}", 1, 2, 3, 4, 5), Times.Once);

            var italianCulture = new CultureInfo("it-It");
            var date = new DateTime(2000, 12, 28, 1, 4, 43, 0);
            _logger.DebugFormat(italianCulture, "Date: {0}", date);
            _mockedLogger.Verify(l => l.DebugFormat(italianCulture, "Date: {0}", date));
        }

        [Test]
        public void When_logging_using_info()
        {
            _logger.Info("Hi");
            _mockedLogger.Verify(l => l.Info("Hi"), Times.Once);

            var ex = new InvalidOperationException();
            _logger.Info("Ooops", ex);
            _mockedLogger.Verify(l => l.Info("Ooops", ex), Times.Once);
        }

        [Test]
        public void When_logging_using_info_format()
        {
            _logger.InfoFormat("E:{0}", 1);
            _mockedLogger.Verify(l => l.InfoFormat("E:{0}", 1), Times.Once);

            _logger.InfoFormat("E:{0}, {1}", 1, 2);
            _mockedLogger.Verify(l => l.InfoFormat("E:{0}, {1}", 1, 2), Times.Once);

            _logger.InfoFormat("E:{0}, {1}, {2}", 1, 2, 3);
            _mockedLogger.Verify(l => l.InfoFormat("E:{0}, {1}, {2}", 1, 2, 3), Times.Once);

            _logger.InfoFormat("E:{0}, {1}, {2}, {3}, {4}", 1, 2, 3, 4, 5);
            _mockedLogger.Verify(l => l.InfoFormat("E:{0}, {1}, {2}, {3}, {4}", 1, 2, 3, 4, 5), Times.Once);

            var italianCulture = new CultureInfo("it-It");
            var date = new DateTime(2000, 12, 28, 1, 4, 43, 0);
            _logger.InfoFormat(italianCulture, "Date: {0}", date);
            _mockedLogger.Verify(l => l.InfoFormat(italianCulture, "Date: {0}", date));
        }

        [Test]
        public void When_logging_using_warn()
        {
            _logger.Warn("Hi");
            _mockedLogger.Verify(l => l.Warn("Hi"), Times.Once);

            var ex = new InvalidOperationException();
            _logger.Warn("Ooops", ex);
            _mockedLogger.Verify(l => l.Warn("Ooops", ex), Times.Once);
        }

        [Test]
        public void When_logging_using_warn_format()
        {
            _logger.WarnFormat("E:{0}", 1);
            _mockedLogger.Verify(l => l.WarnFormat("E:{0}", 1), Times.Once);

            _logger.WarnFormat("E:{0}, {1}", 1, 2);
            _mockedLogger.Verify(l => l.WarnFormat("E:{0}, {1}", 1, 2), Times.Once);

            _logger.WarnFormat("E:{0}, {1}, {2}", 1, 2, 3);
            _mockedLogger.Verify(l => l.WarnFormat("E:{0}, {1}, {2}", 1, 2, 3), Times.Once);

            _logger.WarnFormat("E:{0}, {1}, {2}, {3}, {4}", 1, 2, 3, 4, 5);
            _mockedLogger.Verify(l => l.WarnFormat("E:{0}, {1}, {2}, {3}, {4}", 1, 2, 3, 4, 5), Times.Once);

            var italianCulture = new CultureInfo("it-It");
            var date = new DateTime(2000, 12, 28, 1, 4, 43, 0);
            _logger.WarnFormat(italianCulture, "Date: {0}", date);
            _mockedLogger.Verify(l => l.WarnFormat(italianCulture, "Date: {0}", date));
        }

        [Test]
        public void When_logging_using_error()
        {
            _logger.Error("Hi");
            _mockedLogger.Verify(l => l.Error("Hi"), Times.Once);

            var ex = new InvalidOperationException();
            _logger.Error("Ooops", ex);
            _mockedLogger.Verify(l => l.Error("Ooops", ex), Times.Once);
        }

        [Test]
        public void When_logging_using_error_format()
        {
            _logger.ErrorFormat("E:{0}", 1);
            _mockedLogger.Verify(l => l.ErrorFormat("E:{0}", 1), Times.Once);

            _logger.ErrorFormat("E:{0}, {1}", 1, 2);
            _mockedLogger.Verify(l => l.ErrorFormat("E:{0}, {1}", 1, 2), Times.Once);

            _logger.ErrorFormat("E:{0}, {1}, {2}", 1, 2, 3);
            _mockedLogger.Verify(l => l.ErrorFormat("E:{0}, {1}, {2}", 1, 2, 3), Times.Once);

            _logger.ErrorFormat("E:{0}, {1}, {2}, {3}, {4}", 1, 2, 3, 4, 5);
            _mockedLogger.Verify(l => l.ErrorFormat("E:{0}, {1}, {2}, {3}, {4}", 1, 2, 3, 4, 5), Times.Once);

            var italianCulture = new CultureInfo("it-It");
            var date = new DateTime(2000, 12, 28, 1, 4, 43, 0);
            _logger.ErrorFormat(italianCulture, "Date: {0}", date);
            _mockedLogger.Verify(l => l.ErrorFormat(italianCulture, "Date: {0}", date));
        }

        [Test]
        public void When_logging_using_fatal()
        {
            _logger.Fatal("Hi");
            _mockedLogger.Verify(l => l.Fatal("Hi"), Times.Once);

            var ex = new InvalidOperationException();
            _logger.Fatal("Ooops", ex);
            _mockedLogger.Verify(l => l.Fatal("Ooops", ex), Times.Once);
        }

        [Test]
        public void When_logging_using_fatal_format()
        {
            _logger.FatalFormat("E:{0}", 1);
            _mockedLogger.Verify(l => l.FatalFormat("E:{0}", 1), Times.Once);

            _logger.FatalFormat("E:{0}, {1}", 1, 2);
            _mockedLogger.Verify(l => l.FatalFormat("E:{0}, {1}", 1, 2), Times.Once);

            _logger.FatalFormat("E:{0}, {1}, {2}", 1, 2, 3);
            _mockedLogger.Verify(l => l.FatalFormat("E:{0}, {1}, {2}", 1, 2, 3), Times.Once);

            _logger.FatalFormat("E:{0}, {1}, {2}, {3}, {4}", 1, 2, 3, 4, 5);
            _mockedLogger.Verify(l => l.FatalFormat("E:{0}, {1}, {2}, {3}, {4}", 1, 2, 3, 4, 5), Times.Once);

            var italianCulture = new CultureInfo("it-It");
            var date = new DateTime(2000, 12, 28, 1, 4, 43, 0);
            _logger.FatalFormat(italianCulture, "Date: {0}", date);
            _mockedLogger.Verify(l => l.FatalFormat(italianCulture, "Date: {0}", date));
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

        [Test]
        public void When_logging_using_scopped_logger()
        {
            // First try with Debug
            using (_logger.GetScopedLogger("Foo", LogLevel.Debug))
            {
                _mockedLogger.Verify(l => l.Debug("[-BEGIN---> Foo]"), Times.Once);

                _logger.WarnFormat("bar {0}", "is closed");

                _mockedLogger.Verify(l => l.WarnFormat("bar {0}", "is closed"), Times.Once);
            }

            _mockedLogger.Verify(l => l.Debug("[--END----> Foo]"), Times.Once);

            // Now try Fatal
            using (_logger.GetScopedLogger("DummyScope", LogLevel.Fatal))
            {
                _mockedLogger.Verify(l => l.Fatal("[-BEGIN---> DummyScope]"), Times.Once);

                _logger.WarnFormat("bar {0}", "is opened");

                _mockedLogger.Verify(l => l.WarnFormat("bar {0}", "is opened"), Times.Once);
            }

            _mockedLogger.Verify(l => l.Fatal("[--END----> DummyScope]"), Times.Once);
        }
    }
}