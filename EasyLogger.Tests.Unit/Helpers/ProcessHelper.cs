namespace EasyLogger.Tests.Unit.Helpers
{
    using System.Collections.Generic;
    using System.Diagnostics;

    public static class ProcessHelper
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
                if (args?.Data == null) { return; }

                lock (locker)
                {
                    outputMessages.Add(args.Data);
                }

                Debug.WriteLine("Process Error: " + args.Data);
            };

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            return process;
        }
    }
}
