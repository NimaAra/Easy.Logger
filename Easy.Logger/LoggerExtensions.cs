namespace Easy.Logger
{
    using System;
    using System.Runtime.ExceptionServices;
    using System.Threading.Tasks;

    /// <summary>
    /// Contains a set of helper methods for using <see cref="ILogger"/>.
    /// </summary>
    public static class LoggerExtensions
    {
        /// <summary>
        /// Sets up the exception handling for the app-domain unhandled exceptions
        /// </summary>
        /// <param name="logger">The logger to use in an event of an exception</param>
        [HandleProcessCorruptedStateExceptions]
        public static void SetupAppDomainExceptionLogging(this ILogger logger)
        {
            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                var e = args.ExceptionObject as Exception;
                if (e != null)
                {
                    logger.ErrorFormat("AppDomain Exception - Message: {0}", e.Message);
                    logger.ErrorFormat("AppDomain Exception - Inner exception: {0}", e.InnerException);
                    logger.ErrorFormat("AppDomain Exception - StackTrace: {0}", e.StackTrace);
                }
                else
                {
                    logger.ErrorFormat("AppDomain Exception - Could not cast as Exception: {0}", args.ExceptionObject);
                }
                logger.ErrorFormat("AppDomain Exception - Is terminating: {0}", args.IsTerminating.ToString());
            };
        }

        /// <summary>
        /// Sets up the exception handling for the unobserved task exceptions
        /// </summary>
        /// <param name="logger">The logger to use in an event of an exception</param>
        [HandleProcessCorruptedStateExceptions]
        public static void SetupUnobservedTaskExceptionLogging(this ILogger logger)
        {
            TaskScheduler.UnobservedTaskException += (sender, args) =>
            {
                var e = args.Exception;
                logger.ErrorFormat("TaskScheduler Unobserved Exception - Message: {0}", e.Message);
                logger.ErrorFormat("TaskScheduler Unobserved Exception - Inner exception: {0}", e.InnerException);
                logger.ErrorFormat("TaskScheduler Unobserved Exception - Inner exceptions: {0}", e.InnerExceptions);
                logger.ErrorFormat("TaskScheduler Unobserved Exception - StackTrace: {0}", e.StackTrace);
                args.SetObserved();
            };
        }
    }
}