using System;
using System.Threading;

namespace ThreadPost
{
    public class IntervalUpdateThread : IDisposable
    {
        private IUpdatable _updatable;
        private readonly Thread _t;
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();
        private readonly long _intervalTicks;

        public IntervalUpdateThread(TimeSpan interval, IUpdatable updatable)
        {
            _updatable = updatable;
            _intervalTicks = interval.Ticks;
            var t = _t = new Thread(Process);
            t.Start();
        }

        private void Process()
        {
            _updatable.Initialize();

            var last = DateTime.UtcNow.Ticks;

            while (!_cts.IsCancellationRequested)
            {
                _updatable.Upadte(_cts.Token);

                var current = DateTime.UtcNow.Ticks;

                var next = last + _intervalTicks;
                var ticksToNext = next - current;

                if (ticksToNext > 0)
                {
                    Thread.Sleep(TimeSpan.FromTicks(ticksToNext));
                }

                last = DateTime.UtcNow.Ticks;
            }
        }

        public void Dispose()
        {
            _cts.Cancel();
            _t.Join();
        }
    }
}
