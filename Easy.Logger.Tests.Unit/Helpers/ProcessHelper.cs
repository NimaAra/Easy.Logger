namespace Easy.Logger.Tests.Unit.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    internal static class ProcessHelper
    {
        public static Process GetProcess(string processPath, IList<string> outputMessages)
        {
            var processInfo = new ProcessStartInfo(processPath)
            {
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            var process = new Process
            {
                EnableRaisingEvents = true,
                StartInfo = processInfo
            };

            var locker = new object();

            process.OutputDataReceived += (sender, args) =>
            {
                if (args?.Data == null) { return; }

                lock (locker)
                {
                    outputMessages.Add(args.Data);
                }

                Debug.WriteLine(args.Data);
            };
            process.ErrorDataReceived += (sender, args) =>
            {
                var errMsg = "[Process Error] - ";

                if (args?.Data != null)
                {
                    errMsg += args.Data;
                } else
                {
                    errMsg += "No data available";
                }

                lock (locker)
                {
                    outputMessages.Add(errMsg);
                }

                Console.WriteLine(errMsg);
            };

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            return process;
        }
    }
}
