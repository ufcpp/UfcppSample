using System;
using System.Threading;
using System.Threading.Tasks;

namespace CachedAsync
{
    class TimerLoop
    {
        private readonly AsyncOperation<int> _cache = new AsyncOperation<int>();
        private readonly int _intervalMillisecond;
        private readonly CancellationTokenSource _cts;
        private readonly Task _loop;

        public TimerLoop(int intervalMillisecond)
        {
            _intervalMillisecond = intervalMillisecond;
            _cts = new CancellationTokenSource();
            _loop = Loop();
        }

        private async Task Loop()
        {
            var i = 0;
            while (!_cts.IsCancellationRequested)
            {
                await Task.Delay(_intervalMillisecond);
                _cache.TrySetResult(i);
                ++i;
            }
        }

        public Task StopAsync()
        {
            _cts.Cancel();
            return _loop;
        }

        void Throw() => throw new InvalidOperationException();

        public ValueTask<int> XAsync()
        {
            if (!_cache.TryOwnAndReset()) Throw();
            return _cache.ValueTaskOfT;
        }
    }
}
