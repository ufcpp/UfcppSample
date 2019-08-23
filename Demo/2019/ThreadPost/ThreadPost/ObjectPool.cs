using System;
using System.Collections.Concurrent;

namespace ThreadPost
{
    public class ObjectPool<T>
    {
        private readonly Func<T> _generator;
        private readonly ConcurrentQueue<T> _queue = new ConcurrentQueue<T>();

        public ObjectPool(Func<T> generator) => _generator = generator;

        public T Rent() => _queue.TryDequeue(out var value) ? value : _generator();
        public void Return(T value) => _queue.Enqueue(value);
    }
}
