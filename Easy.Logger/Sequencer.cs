namespace Easy.Logger
{
    using System;
    using System.Collections.Concurrent;
    using System.Runtime.Serialization;
    using System.Threading.Tasks;

    /// <summary>
    /// A single worker implementation of the <c>Producer-Consumer</c> pattern.
    /// </summary>
    /// <typeparam name="T">The type of the object to be produced/consumed</typeparam>
    public sealed class Sequencer<T>
    {
        private readonly BlockingCollection<T> _queue;
        private Task _worker;

        /// <summary>
        /// Creates an instance of <see cref="Sequencer{T}"/>
        /// </summary>
        /// <param name="consumer">The action to be executed when consuming the item.</param>
        public Sequencer(Action<T> consumer)
            : this(-1, consumer) { }

        /// <summary>
        /// Creates an instance of <see cref="Sequencer{T}"/>
        /// </summary>
        /// <param name="consumer">The action to be executed when consuming the item.</param>
        /// <param name="boundedCapacity">
        /// The bounded size of the queue.
        /// Any more items added will block until there is more space available.
        /// </param>
        public Sequencer(Action<T> consumer, int boundedCapacity)
            : this(boundedCapacity, consumer)
        {
            if (boundedCapacity <= 0) { throw new ArgumentException("Bounded capacity should be greater than zero."); }
        }

        private Sequencer(int boundedCapacity, Action<T> consumer)
        {
            if (consumer == null) { throw new ArgumentNullException(nameof(consumer)); }

            _queue = boundedCapacity > 0 ? new BlockingCollection<T>(boundedCapacity) : new BlockingCollection<T>();
            SetupConsumer(consumer, 1);
        }

        /// <summary>
        /// Returns the bounded capacity of the underlying queue. -1 for unbounded.
        /// </summary>
        public int Capacity => _queue.BoundedCapacity;

        /// <summary>
        /// Returns the count of items that are pending consumption.
        /// </summary>
        public uint PendingCount => (uint)_queue.Count;

        /// <summary>
        /// Returns the pending items in the queue. Note, the items are valid as
        /// the snapshot at the time of invocation.
        /// </summary>
        public T[] PendingItems => _queue.ToArray();

        /// <summary>
        /// Gets whether <see cref="Sequencer{T}"/> has started to shutdown.
        /// </summary>
        public bool ShutdownRequested => _queue.IsAddingCompleted;

        /// <summary>
        /// Thrown when the <see cref="_worker"/> throws an exception.
        /// </summary>
        public event EventHandler<SequencerExceptionEventArgs> OnException;

        /// <summary>
        /// Adds the specified item to the <see cref="Sequencer{T}"/>. 
        /// This method blocks if the queue is full and until there is more room.
        /// </summary>
        /// <param name="item">The item to be added.</param>
        /// <exception cref="ObjectDisposedException">The <see cref="Sequencer{T}"/> has been disposed.</exception>
        /// <exception cref="InvalidOperationException">
        /// The underlying collection for <see cref="Sequencer{T}"/> has been marked as 
        /// complete with regards to additions.-or-The underlying collection didn't accept the item.
        /// </exception>
        public void Enqueue(T item)
        {
            _queue.Add(item);
        }

        /// <summary>
        /// Attempts to add the specified item to the <see cref="Sequencer{T}"/>.
        /// </summary>
        /// <param name="item">The item to be added.</param>
        /// <returns>
        /// <c>True</c> if item could be added; otherwise <c>False</c>. 
        /// If the item is a duplicate, and the underlying collection does 
        /// not accept duplicate items, then an InvalidOperationException is thrown.
        /// </returns>
        /// <exception cref="ObjectDisposedException">The <see cref="Sequencer{T}"/> has been disposed.</exception>
        /// <exception cref="InvalidOperationException">
        /// The underlying collection for <see cref="Sequencer{T}"/> has been marked as 
        /// complete with regards to additions.-or-The underlying collection didn't accept the item.
        /// </exception>
        public bool TryEnqueue(T item)
        {
            return _queue.TryAdd(item);
        }

        /// <summary>
        /// Attempts to add the specified item to the <see cref="Sequencer{T}"/>.
        /// </summary>
        /// <param name="item">The item to be added.</param>
        /// <param name="timeout">
        /// A <c>TimeSpan</c> that represents the number of milliseconds 
        /// to wait, or a <c>TimeSpan</c> that represents -1 milliseconds to wait indefinitely.
        /// </param>
        /// <returns>
        /// <c>True</c> if the item could be added to the collection within the specified time span; otherwise, <c>False</c>.
        /// </returns>
        /// <exception cref="ObjectDisposedException">The <see cref="Sequencer{T}"/> has been disposed.</exception>
        /// <exception cref="InvalidOperationException">
        /// The underlying collection for <see cref="Sequencer{T}"/> has been marked as 
        /// complete with regards to additions.-or-The underlying collection didn't accept the item.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="timeout"/> is a negative number other than -1 milliseconds, 
        /// which represents an infinite time-out -or- <paramref name="timeout"/> is greater than MaxValue.
        /// </exception>
        public bool TryEnqueue(T item, TimeSpan timeout)
        {
            return _queue.TryAdd(item, timeout);
        }

        /// <summary>
        /// Marks the <see cref="Sequencer{T}"/> instance as not accepting 
        /// any more items. Any outstanding items will be consumed.
        /// </summary>
        /// <param name="waitForPendingItems">
        /// Flag indicating whether to wait for pending items to be processed.
        /// </param>
        public void Shutdown(bool waitForPendingItems = true)
        {
            _queue.CompleteAdding();
            if (waitForPendingItems) { _worker.Wait(); }
        }

        private void SetupConsumer(Action<T> consumer, int workerCount)
        {
            Action work = () =>
            {
                foreach (var item in _queue.GetConsumingEnumerable()) { consumer(item); }
            };

            for (var i = 0; i < workerCount; i++)
            {
                var task = new Task(work, TaskCreationOptions.LongRunning);
                task.HandleExceptions(e =>
                {
                    OnException.Raise(this, new SequencerExceptionEventArgs(new SequencerException("Exception occurred.", e)));
                });

                _worker = task;
                _worker.Start();
            }
        }
    }

    /// <summary>
    /// This class used as a container for when an <see cref="System.Exception"/> 
    /// is raised by the <see cref="Sequencer{T}"/>
    /// </summary>
    public sealed class SequencerExceptionEventArgs : EventArgs
    {
        /// <summary>
        /// Creates an instance of the <see cref="Sequencer{T}"/>
        /// </summary>
        /// <param name="e">The <see cref="System.Exception"/></param>
        public SequencerExceptionEventArgs(SequencerException e)
        {
            Exception = e;
        }

        /// <summary>
        /// The <see cref="System.Exception"/> raised by the <see cref="Sequencer{T}"/>.
        /// </summary>
        public SequencerException Exception { get; }
    }

    /// <summary>
    /// The <see cref="System.Exception"/> thrown by the <see cref="Sequencer{T}"/>.
    /// </summary>
    [Serializable]
    public sealed class SequencerException : Exception
    {
        /// <summary>
        /// Creates an instance of the <see cref="SequencerException"/>.
        /// </summary>
        public SequencerException() { }

        /// <summary>
        /// Creates an instance of the <see cref="SequencerException"/>.
        /// </summary>
        /// <param name="message">The message for the <see cref="Exception"/></param>
        public SequencerException(string message) : base(message) { }

        /// <summary>
        /// Creates an instance of the <see cref="SequencerException"/>.
        /// </summary>
        /// <param name="message">The message for the <see cref="Exception"/></param>
        /// <param name="innerException">The inner exception</param>
        public SequencerException(string message, Exception innerException) : base(message, innerException) { }

        /// <summary>
        /// Creates an instance of the <see cref="SequencerException"/>.
        /// </summary>
        /// <param name="info">The serialization information</param>
        /// <param name="context">The streaming context</param>
        public SequencerException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
