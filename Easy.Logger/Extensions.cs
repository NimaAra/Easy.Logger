namespace Easy.Logger
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public static class Extensions
    {
        /// <summary>
        /// A convenience method to raise an <see cref="EventHandler{TEventArgs}"/>.
        /// </summary>
        /// <typeparam name="T">Type of the event.</typeparam>
        /// <param name="handler">A copy of the handlers to call, note that <paramref name="handler"/> is copied by value.</param>
        /// <param name="sender">Sender of the <see cref="EventHandler{TEventArgs}"/></param>
        /// <param name="args">Arguments to raise as part of the <see cref="EventHandler{TEventArgs}"/></param>
        public static void Raise<T>(this EventHandler<T> handler, object sender, T args) where T : EventArgs
        {
            // We don't need to have a local copy of the handler as they are passed by value.
            // it is however slightly less efficient than just a local copy at the calling method
            // due to the extra method overhead, also you would have to pass in an event args
            // regardless of whether a subscriber is attached or not.
            handler?.Invoke(sender, args);
        }

        /// <summary>
        /// Handles all the exceptions thrown by the <paramref name="task"/>
        /// </summary>
        /// <param name="task">The task which might throw exceptions.</param>
        /// <param name="exceptionsHandler">The handler to which every exception is passed.</param>
        /// <returns>The continuation task added to <paramref name="task"/>.</returns>
        public static Task HandleExceptions(this Task task, Action<Exception> exceptionsHandler)
        {
            return task.ContinueWith(t =>
            {
                var e = t.Exception;

                if (e == null) { return; }

                e.Flatten().Handle(ie =>
                {
                    exceptionsHandler(ie);
                    return true;
                });
            },
            CancellationToken.None,
            TaskContinuationOptions.ExecuteSynchronously,
            TaskScheduler.Default);
        }
    }
}