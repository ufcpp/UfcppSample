using System;
using System.Collections.Concurrent;
using System.Threading;

namespace ThreadPost
{
    public class UpdatableSynchronizationContext : SynchronizationContext, IUpdatable
    {
        public override void Post(SendOrPostCallback d, object state) => _queue.Enqueue(new SynchronizationContextPostItem(d, state));

        private ConcurrentQueue<SynchronizationContextPostItem> _queue = new ConcurrentQueue<SynchronizationContextPostItem>();

        public void Initialize() => SetSynchronizationContext(this);
        public void Upadte(CancellationToken cancellationToken)
        {
            while (!_queue.IsEmpty && !cancellationToken.IsCancellationRequested)
            {
                if (_queue.TryDequeue(out var t))
                {
                    t.Invoke(UnhandledException);
                }
            }
        }

        public event UnhandledExceptionEventHandler UnhandledException;
    }
}
