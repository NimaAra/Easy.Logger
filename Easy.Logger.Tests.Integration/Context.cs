namespace Easy.Logger.Tests.Integration
{
    using System;
    using System.Collections.Concurrent;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Xml.Linq;
    using Easy.Logger.Interfaces;
    using Easy.Logger.Tests.Integration.Models;
    using NUnit.Framework;
    using Shouldly;

    [TestFixture]
    internal class Context
    {
        private ILogService _logService;
        private FileInfo _configFile, _logFile;
        private EasyLogListener _listener;
        private ConcurrentQueue<LogPayload> _receivedPayloads;

        [SetUp]
        public void Setup()
        {
            _logService = Log4NetService.Instance;

            _configFile = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "integration.tests-log4net.config"));
            _logFile = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs", "Integration.Tests.log"));
            
            if (_logFile.Exists) { _logFile.Delete(); }

            _logService.Configure(_configFile);

            _receivedPayloads = new ConcurrentQueue<LogPayload>();
            
            StartHttpServer();
        }

        [Test]
        public void Run()
        {
            var logger = _logService.GetLogger(GetType());
            logger.Debug("Something is about to happen...");

            // Should not be replaced by Task.Delay as
            // Thread name will be changed after await
            // which will fail the log content assertions
            Thread.Sleep(TimeSpan.FromSeconds(1));

            logger.InfoFormat("What's your number? It's: {0}", 1234);

            logger.Error("Ooops I did it again!", new ArgumentNullException("cooCoo"));

            logger.FatalFormat("Going home now - {0}", new ApplicationException("CiaoCiao"));

            Thread.Sleep(TimeSpan.FromSeconds(1));
        }

        [TearDown]
        public void TestTearDown()
        {
            _logService.Dispose();
            _listener.Dispose();

            CheckLogFileContent(_logFile);
            CheckReceivedPayloads(_receivedPayloads.ToArray());
        }

        private static void CheckLogFileContent(FileSystemInfo logFile)
        {
            var dateTimeRegex = new Regex(@"^\[\d{4}-\d{2}-\d{2}\s\d{2}:\d{2}:\d{2}\,\d{3}\]\s\[", RegexOptions.Compiled);

            var lines = File.ReadAllLines(logFile.FullName);
            lines.Length.ShouldBe(6);

            foreach (var line in lines)
            {
                if (line != "System.ArgumentNullException: Value cannot be null."
                    && line != "Parameter name: cooCoo")
                {
                    dateTimeRegex.IsMatch(line).ShouldBeTrue();
                }
            }

            lines[0].ShouldEndWith(" [DEBUG] [NonParallelWorker] [Context] - Something is about to happen...");
            lines[1].ShouldEndWith(" [INFO ] [NonParallelWorker] [Context] - What's your number? It's: 1234");
            lines[2].ShouldEndWith(" [ERROR] [NonParallelWorker] [Context] - Ooops I did it again!");
            lines[3].ShouldBe("System.ArgumentNullException: Value cannot be null.");
            lines[4].ShouldBe("Parameter name: cooCoo");
            lines[5].ShouldEndWith(" [FATAL] [NonParallelWorker] [Context] - Going home now - System.ApplicationException: CiaoCiao");
        }

        private static void CheckReceivedPayloads(LogPayload[] payloads)
        {
            int pid;
            string processName;
            using (var p = Process.GetCurrentProcess())
            {
                pid = p.Id;
                processName = p.ProcessName;
            }
            
            payloads.Length.ShouldBe(2);

            payloads[0].BatchCount.ShouldBe(1);
            payloads[0].PID.ShouldBe(pid);
            payloads[0].ProcessName.ShouldBe(processName);
            payloads[0].Host.ShouldBe(Dns.GetHostName());
            payloads[0].TimestampUTC.ShouldBeOfType<DateTimeOffset>();
            payloads[0].TimestampUTC.ShouldNotBe(default(DateTimeOffset));
            payloads[0].Sender.ShouldBe("Integration.Tests");
            payloads[0].Entries.Length.ShouldBe(1);

            payloads[1].BatchCount.ShouldBe(2);
            payloads[1].PID.ShouldBe(pid);
            payloads[1].ProcessName.ShouldBe(processName);
            payloads[1].Host.ShouldBe(Dns.GetHostName());
            payloads[1].TimestampUTC.ShouldBeOfType<DateTimeOffset>();
            payloads[1].TimestampUTC.ShouldNotBe(default(DateTimeOffset));
            payloads[1].Sender.ShouldBe("Integration.Tests");
            payloads[1].Entries.Length.ShouldBe(3);

            var allEntries = payloads.SelectMany(p => p.Entries).ToArray();
            
            allEntries[0].DateTimeOffset.ShouldBeOfType<DateTimeOffset>();
            allEntries[0].DateTimeOffset.ShouldNotBe(default(DateTimeOffset));
            allEntries[0].Level.ShouldBe("DEBUG");
            allEntries[0].ThreadID.ShouldBe("NonParallelWorker");
            allEntries[0].LoggerName.ShouldBe("Easy.Logger.Tests.Integration.Context");
            allEntries[0].Message.ShouldBe("Something is about to happen...");
            allEntries[0].Exception.ShouldBeNull();

            allEntries[1].DateTimeOffset.ShouldBeOfType<DateTimeOffset>();
            allEntries[1].DateTimeOffset.ShouldNotBe(default(DateTimeOffset));
            allEntries[1].Level.ShouldBe("INFO");
            allEntries[1].ThreadID.ShouldBe("NonParallelWorker");
            allEntries[1].LoggerName.ShouldBe("Easy.Logger.Tests.Integration.Context");
            allEntries[1].Message.ShouldBe("What's your number? It's: 1234");
            allEntries[1].Exception.ShouldBeNull();

            allEntries[2].DateTimeOffset.ShouldBeOfType<DateTimeOffset>();
            allEntries[2].DateTimeOffset.ShouldNotBe(default(DateTimeOffset));
            allEntries[2].Level.ShouldBe("ERROR");
            allEntries[2].ThreadID.ShouldBe("NonParallelWorker");
            allEntries[2].LoggerName.ShouldBe("Easy.Logger.Tests.Integration.Context");
            allEntries[2].Message.ShouldBe("Ooops I did it again!");
            allEntries[2].Exception.ShouldNotBeNull();
            
            allEntries[2].Exception.Data.ShouldNotBeNull();
            allEntries[2].Exception.HResult.ShouldBe(-2147467261);
            allEntries[2].Exception.InnerException.ShouldBeNull();
            allEntries[2].Exception.Message.ShouldBe("Value cannot be null.");
            allEntries[2].Exception.Source.ShouldBeNull();
            allEntries[2].Exception.StackTrace.ShouldBeNull();

            allEntries[3].DateTimeOffset.ShouldBeOfType<DateTimeOffset>();
            allEntries[3].DateTimeOffset.ShouldNotBe(default(DateTimeOffset));
            allEntries[3].Level.ShouldBe("FATAL");
            allEntries[3].ThreadID.ShouldBe("NonParallelWorker");
            allEntries[3].LoggerName.ShouldBe("Easy.Logger.Tests.Integration.Context");
            allEntries[3].Message.ShouldBe("Going home now - System.ApplicationException: CiaoCiao");
            allEntries[3].Exception.ShouldBeNull();

            Array.ForEach(payloads, Console.Write);
        }

        private void StartHttpServer()
        {
            var endpointStr = XDocument.Load(_configFile.FullName)
                .Descendants()
                .Elements()
                .Single(e => e.Attributes().Any(a => a.Name == "name" && a.Value == "HTTPAppender"))
                .Elements("endpoint")
                .Single()
                .Attribute("value")?.Value;

            _listener = new EasyLogListener(new Uri(endpointStr ?? throw new InvalidOperationException()));

            _listener.OnError += (sender, exception) => throw exception;
            _listener.OnPayload += (sender, payload) => _receivedPayloads.Enqueue(payload);
            _listener.Start();
        }
    }
}