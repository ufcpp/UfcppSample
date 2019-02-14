namespace Preview3.PatternBasedForeach
{
    // Preview 3 はほぼバグ修正程度の変更しかされてなさそう。
    // とりあえず、
    // - pattern-based で foreach 後の Dispose が呼ばれるようになった
    // - DisposeAsync の戻り値が ValueTask である必要がなくなった

    using System;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;

    // ref struct の場合、IDisposable を実装しなくても pattern-based で Dispose メソッドを呼んでもらえる。
    ref struct MyEnumerable
    {
        public MyEnumerable GetEnumerator() => this;
        public int Current => 0;
        public bool MoveNext() => false;
        public void Dispose() => Console.WriteLine("disposed");
    }

    // MyEnumerable と比べて、 ref を取っただけ
    struct MyBrokenEnumerable
    {
        public MyBrokenEnumerable GetEnumerator() => this;
        public int Current => 0;
        public bool MoveNext() => false;

        // この Dispose は呼ばれなくなる。
        // ref struct でない場合、IDisposable インターフェイスの実装が必須
        public void Dispose() => Console.WriteLine("disposed");
    }

    // Async 版
    // MoveNextAsync, DisposeAsync の戻り値は ValueTask じゃなくてもいい。
    // Async 版の場合は無条件に pattern-based に DisposeAsync を呼んでもらえる。。
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
        static void Main()
        {
            Console.WriteLine(nameof(MyEnumerable));
            foreach (var x in new MyEnumerable()) { }

            Console.WriteLine(nameof(MyBrokenEnumerable));
            foreach (var x in new MyBrokenEnumerable()) { }

            M().Wait();
        }

        static async Task M()
        {
            Console.WriteLine(nameof(MyAsyncEnumerable));
            await foreach (var x in new MyAsyncEnumerable()) { }
        }
    }
}
