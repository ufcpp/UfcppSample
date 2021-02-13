using BenchmarkDotNet.Attributes;
using System;
using System.Threading.Tasks;

namespace NoThrowAwaiter
{
    public class IgnoreCancelBenchmark
    {
        private const int N = 200;

        [Benchmark]
        public async Task TryCatch()
        {
            for (int i = 0; i < N; i++)
            {
                try
                {
                    await Throw().ConfigureAwait(false);
                }
                catch (OperationCanceledException)
                {
                    // ignore cancel
                }
            }
        }

        [Benchmark]
        public async Task NoThrow()
        {
            for (int i = 0; i < N; i++)
            {
                await Throw().NoThrowOnCancel(false);
            }
        }

        private static async Task Throw()
        {
            await Task.Yield();
            await Task.Yield();
            await Task.Yield();
            throw new TaskCanceledException();
        }
    }
}
