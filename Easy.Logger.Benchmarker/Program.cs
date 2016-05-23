namespace Easy.Logger.Benchmarker
{
    using System;
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;

    internal class Program
    {
        private readonly TimeSpan _duration;
        private readonly ILogger _logger;

        private static void Main(string[] args)
        {
            var duration = TimeSpan.FromSeconds(10);

            if (args.Length > 0)
            {
                var seconds = int.Parse(args[0]);
                duration = TimeSpan.FromSeconds(seconds);
            }

            new Program(duration).Run();
        }

        public Program(TimeSpan duration)
        {
            _duration = duration;
            _logger = Log4NetService.Instance.GetLogger<Program>();
        }

        private void Run()
        {
            _logger.Info("Benchmarking starting");
            TestThroughput();
//            TestMultiThreading();
//            TestIdle();
            _logger.Warn("Benchmarking ended");

            Console.WriteLine("Gen 0: {0}", GC.CollectionCount(0));
            Console.WriteLine("Gen 1: {0}", GC.CollectionCount(1));
            Console.WriteLine("Gen 2: {0}", GC.CollectionCount(2));
        }

        private void TestThroughput()
        {
            Console.WriteLine("------------Testing Throughput------------");

            var sw = Stopwatch.StartNew();
            long counter = 0;
            while (sw.Elapsed < _duration)
            {
                counter++;
                _logger.DebugFormat("Counter is: {0}", counter);
            }

            Console.WriteLine("Counter reached: {0:n0}, Time Taken: {1}", counter, sw.Elapsed);
        }

        private void TestMultiThreading()
        {
            Console.WriteLine("------------Testing MultiThreading------------");

            const int WorkerCount = 6;

            long totalCounter = 0;
            Func<int> action = () =>
            {
                var localCounter = 0;
                var sw = Stopwatch.StartNew();

                while (sw.Elapsed < _duration)
                {
                    _logger.DebugFormat("Counter is: {0}", ++localCounter);
                }

                return localCounter;
            };

            var totalSw = Stopwatch.StartNew();

            Parallel.For(
                0,
                WorkerCount,
                new ParallelOptions { MaxDegreeOfParallelism = WorkerCount },
                () => 0,
                (i, state, partial) => action(),
                partialCounter => Interlocked.Add(ref totalCounter, partialCounter));

            Console.WriteLine("Counter reached: {0:n0}, Time Taken: {1}", totalCounter, totalSw.Elapsed);
        }

        private void TestIdle()
        {
            Console.WriteLine("------------Testing Idle------------");

            var sw = Stopwatch.StartNew();
            long counter = 0;
            while (sw.Elapsed < _duration)
            {
                counter++;
                _logger.DebugFormat("Counter is: {0}", counter);
                Thread.Sleep(TimeSpan.FromSeconds(1));
            }

            Console.WriteLine("Counter reached: {0:n0}, Time Taken: {1}", counter, sw.Elapsed);
        }
    }
}
