namespace Preview2.PatternBasedAsyncForeach
{
    // Preview 3 はほぼバグ修正程度の変更しかされてなさそう。
    // とりあえず、
    // - pattern-based で foreach 後の Dispose が呼ばれるようになった
    // - DisposeAsync の戻り値が ValueTask である必要がなくなった

    using System;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;

    // 非同期 foreach/非同期 using
    // MoveNextAsync, DisposeAsync の戻り値は ValueTask じゃなくてもいい。
    // Async 版の場合は無条件に pattern-based に DisposeAsync を呼んでもらえる。
    class MyAsyncEnumerable
    {
        public MyAsyncEnumerable GetAsyncEnumerator() => this;
        public int Current => 0;

        // ValueTask<bool> ではなく任意の awaitable が使える。
        public MyAwaitable<bool> MoveNextAsync() => default;

        // 同上。任意の awaitable が使える。
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
            await foreach (var x in new MyAsyncEnumerable()) { }
        }
    }
}
