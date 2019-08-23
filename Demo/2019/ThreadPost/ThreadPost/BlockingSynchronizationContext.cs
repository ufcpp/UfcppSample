using System;
using System.Collections.Concurrent;
using System.Threading;

namespace ThreadPost
{
    public class BlockingSynchronizationContext : SynchronizationContext, IDisposable
    {
        private readonly Thread _t;
        private CancellationTokenSource _cts = new CancellationTokenSource();

        public BlockingSynchronizationContext()
        {
            var t = _t = new Thread(Process);
            t.Start();
        }

        public override void Post(SendOrPostCallback d, object state) => _queue.Add(new SynchronizationContextPostItem(d, state));

        private BlockingCollection<SynchronizationContextPostItem> _queue = new BlockingCollection<SynchronizationContextPostItem>();

        private void Process()
        {
            SetSynchronizationContext(this);

            while (!_cts.IsCancellationRequested)
            {
                try
                {
                    if (_queue.TryTake(out var t, 1000, _cts.Token))
                    {
                        t.Invoke(UnhandledException);
                    }
                }
                catch (OperationCanceledException) { }
            }
        }

        public void Dispose()
        {
            _cts.Cancel();
            _t.Join();
        }

        public event UnhandledExceptionEventHandler UnhandledException;
    }
}
