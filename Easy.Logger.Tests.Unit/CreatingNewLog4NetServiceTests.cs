namespace Easy.Logger.Tests.Unit
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Compression;
    using System.Text;
    using System.Threading.Tasks;
    using Helpers;
    using NUnit.Framework;
    using Shouldly;
    using Log4NetService = Log4NetService;

    [TestFixture]
    public sealed class CreatingNewLog4NetServiceTests
    {
        private readonly string _sampleAppName = "Easy.Logger.Tests.SampleLoggerApp.exe";
        private readonly string _logfileName = "--LOGFILE--.log";

        [Test]
        public async Task When_creating_a_log4net_service_with_no_default_configuration()
        {
            var extractedApp = ExtractSampleApp();
            var pathToSampleApp = Path.Combine(extractedApp.FullName, _sampleAppName);

            var outputMessages = new List<string>();
            using (var process = ProcessHelper.GetProcess(pathToSampleApp, outputMessages))
            {
                process.WaitForExit(500);

                outputMessages.ShouldNotBeEmpty();
                outputMessages.ShouldContain(s => s == "[Process Error] - Unhandled Exception: System.IO.FileNotFoundException: Could not find a valid log4net configuration file");

                if (!process.HasExited) { process.Kill(); }
            }

            await Task.Delay(TimeSpan.FromSeconds(2));
            extractedApp.Delete(true);
        }

        [Test]
        public async Task When_creating_a_log4net_service_with_default_configuration()
        {
            var extractedApp = ExtractSampleApp();
            var pathToSampleApp = Path.Combine(extractedApp.FullName, _sampleAppName);

            var log4NetConfigFile = new FileInfo(Path.Combine(extractedApp.FullName, "log4net.config"));
            log4NetConfigFile.Exists.ShouldBeFalse();

            File.WriteAllText(log4NetConfigFile.FullName, GetLog4NetConfiguration(), Encoding.UTF8);

            log4NetConfigFile.Refresh();
            log4NetConfigFile.Exists.ShouldBeTrue();

            var logFile = new FileInfo(Path.Combine(extractedApp.FullName, _logfileName));
            logFile.Exists.ShouldBeFalse();

            var errorMessages = new List<string>();
            using (var process = ProcessHelper.GetProcess(pathToSampleApp, errorMessages))
            {
                process.WaitForExit();
                process.ExitCode.ShouldBe(0);

                logFile.Refresh();
                logFile.Exists.ShouldBeTrue();

                var entries = File.ReadAllLines(logFile.FullName);
                entries.Length.ShouldBe(2);
                entries[0].ShouldContain(" [INFO ] [ 1] Program - I am logging....");
                entries[1].ShouldContain(" [WARN ] [ 1] Program - I am done logging!");
            }

            await Task.Delay(TimeSpan.FromSeconds(2));
            extractedApp.Delete(true);
        }

        [Test]
        public async Task When_creating_a_log4net_service_with_default_configuration_and_then_confuguring_it_again()
        {
            var baseDirectory = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);

            var defaultLogFile = new FileInfo(Path.Combine(baseDirectory.FullName, _logfileName));
            defaultLogFile.Delete();

            // Let's start by default config
            var log4NetConfigFile = new FileInfo(Path.Combine(baseDirectory.FullName, "log4net.config"));
            log4NetConfigFile.Delete();
            File.WriteAllText(log4NetConfigFile.FullName, GetLog4NetConfiguration(), Encoding.UTF8);

            log4NetConfigFile.Refresh();
            log4NetConfigFile.Exists.ShouldBeTrue();

            var logService = Log4NetService.Instance;
            var logger = logService.GetLogger("Test");

            defaultLogFile.Refresh();
            defaultLogFile.Exists.ShouldBeTrue();

            logger.Debug("I am in the default log file.");

            // Let's create a new log4net config file
            const string NewLogFileName = "NewLogFile.log";
            var newLogFile = new FileInfo(Path.Combine(baseDirectory.FullName, NewLogFileName));
            newLogFile.Delete();

            var newConfigContent = GetLog4NetConfiguration().Replace(_logfileName, NewLogFileName);
            var newConfigFile = new FileInfo(Path.Combine(baseDirectory.FullName, "log4net.config"));
            File.WriteAllText(newConfigFile.FullName, newConfigContent, Encoding.UTF8);

            logService.Configure(newConfigFile);

            newLogFile.Refresh();
            newLogFile.Exists.ShouldBeTrue();

            logger = logService.GetLogger(GetType());
            logger.Debug("I am in the new log file.");

            // Now let's test updating an existing config file
            const string NewerLogFileName = "NewerLogFile.log";
            var newerLogFile = new FileInfo(Path.Combine(baseDirectory.FullName, NewerLogFileName));
            newerLogFile.Delete();

            var updatedConfigContent = GetLog4NetConfiguration().Replace(_logfileName, NewerLogFileName);
            File.WriteAllText(newConfigFile.FullName, updatedConfigContent, Encoding.UTF8);

            logService.Configure(newConfigFile);

            logger = logService.GetLogger<CreatingNewLog4NetServiceTests>();
            logger.Debug("I am in the newer log file.");

            newerLogFile.Refresh();
            newerLogFile.Exists.ShouldBeTrue();

            logService.Dispose();

            var defaultLogFileContent = File.ReadAllText(defaultLogFile.FullName);
            var newLogFileContent = File.ReadAllText(newLogFile.FullName);
            var newerLogFileContent = File.ReadAllText(newerLogFile.FullName);

            defaultLogFileContent.ShouldContain("Test - I am in the default log file.");
            newLogFileContent.ShouldContain("CreatingNewLog4NetServiceTests - I am in the new log file.");
            newerLogFileContent.ShouldContain("CreatingNewLog4NetServiceTests - I am in the newer log file.");

            await Task.Delay(TimeSpan.FromSeconds(2));
            
            log4NetConfigFile.Delete();
            newConfigFile.Delete();
            defaultLogFile.Delete();
            newLogFile.Delete();
            newerLogFile.Delete();
        }

        private static DirectoryInfo ExtractSampleApp()
        {
            var pathToArchive = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SampleLoggerApp.zip");

            var tmpDir = Path.GetTempPath();
            var randomDir = Path.GetRandomFileName();

            var extractDir = new DirectoryInfo(Path.Combine(tmpDir, randomDir));

            Console.WriteLine("Extracting to: {0}", extractDir.FullName);
            ZipFile.ExtractToDirectory(pathToArchive, extractDir.FullName);

            return extractDir;
        }
        
        private static string GetLog4NetConfiguration()
        {
            return @"<?xml version=""1.0"" encoding=""utf-8"" ?>
                     <log4net>
                         <root>
                         <level value=""ALL""/>
                         <appender-ref ref=""Default""/>
                         </root>
                     
                         <appender name=""Default"" type=""Easy.Logger.AsyncForwardingAppender"">
                         <appender-ref ref=""RollingFile""/>
                         </appender>
                     
                         <appender name=""RollingFile"" type=""log4net.Appender.RollingFileAppender"">
                         <file type=""log4net.Util.PatternString"" value=""--LOGFILE--.log"" />
                         <appendToFile value=""false""/>
                         <rollingStyle value=""Composite""/>
                         <maxSizeRollBackups value=""-1""/>
                         <maximumFileSize value=""50MB""/>
                         <staticLogFileName value=""true""/>
                         <datePattern value=""yyyy-MM-dd""/>
                         <layout type=""log4net.Layout.PatternLayout"">
                             <conversionPattern value=""%date{ISO8601} [%-5level] [%2thread] %logger{1} - %message%newline%exception""/>
                         </layout>
                         </appender>
                     </log4net>";
        }
    }
}
