namespace Preview2.PatternBasedAsyncUsing
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;

    // 非同期 using は別に IAsyncDisposable インターフェイスの実装を求めない。
    class AsyncDisposable
    {
        // ちゃんと await using のブロックの最後で呼ばれる。
        // 戻り値の型が Task や ValueTask である必要もない。
        public MyAwaitable DisposeAsync()
        {
            Console.WriteLine("disposed async");
            return default;
        }
    }

    struct MyAwaitable<T> { public ValueTaskAwaiter<T> GetAwaiter() => default; }
    struct MyAwaitable { public ValueTaskAwaiter GetAwaiter() => default; }

    class Program
    {
        static async Task Main()
        {
            await using(new AsyncDisposable())
            {
                Console.WriteLine("iside using");
            }
        }
    }
}
