using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadPost
{
    class A
    {
        private ManualResetValueTaskSource<int> _source = new ManualResetValueTaskSource<int>();
        private short _lastToken;

        public async IAsyncEnumerable<int> GetStream()
        {
            while (true)
            {
                _source.Reset();
                _lastToken = _source.Version;
                yield return await new ValueTask<int>(_source, _source.Version);
            }
        }

        public bool TrySet(int value)
        {
            if (_lastToken != _source.Version) return false;
            if (_source.GetStatus(_source.Version) != System.Threading.Tasks.Sources.ValueTaskSourceStatus.Pending) return false;
            _source.SetResult(value);
            return true;
        }

        private TaskCompletionSource<object> _tcs = new TaskCompletionSource<object>();
        public Task Task => _tcs.Task;

        public void Complete() => _tcs.TrySetResult(null);
    }

    class Program
    {
        static void Main()
        {
            const double IntervalSeconds = 0.01;
            const int N = 1000;

            var result = new (int id1, int id2, int i, int j)[N];
            var a = new A();
            var t1 = new UpdatableSynchronizationContext();
            using var u = new IntervalUpdateThread(TimeSpan.FromSeconds(IntervalSeconds), t1);
            using var t2 = new BlockingSynchronizationContext();

            t1.Post(async _ =>
            {
                for (int i = 0; i < N; i++)
                {
                    while (!a.TrySet(i)) await Task.Delay(1);

                    var j = await t2.PostAsync(i => i * Thread.CurrentThread.ManagedThreadId, (long)i);

                    result[i].id1 = Thread.CurrentThread.ManagedThreadId;
                    result[i].j = (int)j;
                }
            }, null);

            t2.Post(async _ =>
            {
                await foreach (var i in a.GetStream())
                {
                    result[i].id2 = Thread.CurrentThread.ManagedThreadId;
                    result[i].i = i;

                    if (i == N - 1) break;
                }

                a.Complete();
            }, null);

            Console.WriteLine("waiting");
            a.Task.Wait();
            Console.WriteLine("done");

            foreach (var (id1, id2, i, j) in result)
            {
                Console.WriteLine((id1, id2, (j + id2) / (i + 1) - id2));
            }

            Console.WriteLine(SynchronizationContextAsyncExtensions.GetFuncDiagCount<long, long>());
        }
    }
}
