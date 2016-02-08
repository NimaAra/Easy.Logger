namespace Easy.Logger
{
    using System.Collections.Generic;

    public sealed class RingBuffer<T>
    {
        private readonly object _locker = new object();
        private readonly Queue<T> _buffer;

        public uint Capacity { get; }
        public bool IsLossy { get; }

        public uint Count
        {
            get
            {
                lock (_locker) { return (uint)_buffer.Count; }
            }
        }
        
        public RingBuffer(uint capacity, bool isLossy = false)
        {
            Capacity = capacity;
            IsLossy = isLossy;
            _buffer = new Queue<T>((int)capacity);
        }

        public bool TryEnqueue(T item)
        {
            lock (_locker)
            {
                if (_buffer.Count < Capacity)
                {
                    _buffer.Enqueue(item);
                    return true;
                }

                if (!IsLossy) { return false; } 

                _buffer.Enqueue(item);
                _buffer.Dequeue();
                return true;
            }
        }

        public bool TryDequeue(out T item)
        {
            lock (_locker)
            {
                if (_buffer.Count > 0)
                {
                    item = _buffer.Dequeue();
                    return true;
                }

                item = default(T);
                return false;
            }
        }

        public void MoveTo(T[] array)
        {
            lock (_locker)
            {
                _buffer.CopyTo(array, 0);
                _buffer.Clear();
            }
        }

        public T[] ToArray()
        {
            lock (_locker)
            {
                var result = _buffer.ToArray();
                _buffer.Clear();
                return result;
            }
        }
    }
}