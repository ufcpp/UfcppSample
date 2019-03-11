namespace Preview2.AsyncStreamsOptionalParamater
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    // awai foreach はデフォルト引数と params 引数がついてる GetAsyncEnumerator を選んでくれる。
    // インターフェイスの明示的実装よりも優先度は上。
    //
    // 拡張メソッドは相変わらず await foreach でもダメ。インターフェイスよりも優先度を低くするなら拡張メソッドも対応させてもいいかもしれないけど、
    // 他の機能(クエリ式(Select, Where, ...)、await (GetAwaiter)、分解(Deconstruct)、コレクション初期化子(Add) との整合性が取れないので悩ましいらしい。
    //
    // 元々あった foreach は結局、破壊的変更になるから仕様を変えれなかったらしい。
    // デフォルト引数、params 引数は受け付けないまま。

    public struct DummyAsyncEnemerator : IAsyncEnumerator<int>
    {
        public DummyAsyncEnemerator(string message) => Console.WriteLine(message);
        public int Current => 0;
        public ValueTask<bool> MoveNextAsync() => new ValueTask<bool>(false);
        public ValueTask DisposeAsync() => default;
    }

    // インターフェイスの実装は要らない。パターンベース。
    struct A
    {
        public DummyAsyncEnemerator GetAsyncEnumerator(CancellationToken cancellationToken = default) => new DummyAsyncEnemerator("A instance"); // 1個しかないので当然これが呼ばれる
    }

    // インスタンスメソッドとインターフェイス明示的実装がある場合、インスタンスメソッド優先
    struct B : IAsyncEnumerable<int>
    {
        public DummyAsyncEnemerator GetAsyncEnumerator(CancellationToken cancellationToken = default) => new DummyAsyncEnemerator("B instance"); // こっちが呼ばれる
        IAsyncEnumerator<int> IAsyncEnumerable<int>.GetAsyncEnumerator(CancellationToken cancellationToken) => new DummyAsyncEnemerator("B explicit interface");
    }

    // 同期版の foreach と違って、await foreach はデフォルト引数/paramsを見てくれる。
    // しかも、インターフェイス明示的実装よりも優先度高い。
    struct C : IAsyncEnumerable<int>
    {
        public DummyAsyncEnemerator GetAsyncEnumerator(CancellationToken cancellationToken = default, int dummy = 0) => new DummyAsyncEnemerator("C instance"); // こっちが呼ばれる
        IAsyncEnumerator<int> IAsyncEnumerable<int>.GetAsyncEnumerator(CancellationToken cancellationToken) => new DummyAsyncEnemerator("C explicit interface");
    }

    // C 参照。 params 版。
    struct D : IAsyncEnumerable<int>
    {
        public DummyAsyncEnemerator GetAsyncEnumerator(CancellationToken cancellationToken = default, params int[] dummy) => new DummyAsyncEnemerator("D instance"); // こっちが呼ばれる
        IAsyncEnumerator<int> IAsyncEnumerable<int>.GetAsyncEnumerator(CancellationToken cancellationToken) => new DummyAsyncEnemerator("D explicit interface");
    }

    // 拡張メソッドは見てくれない。
    // これは foreach で使おうとするとコンパイル時にエラー
    struct E
    {
    }

    static class Ex
    {
        public static DummyAsyncEnemerator GetAsyncEnumerator(this E x, CancellationToken cancellationToken = default) => new DummyAsyncEnemerator("E extension");
    }

    class Program
    {
        static void Main()
        {
            M();
        }

        private static async ValueTask M()
        {
            await foreach (var x in new A()) { }
            await foreach (var x in new B()) { }
            await foreach (var x in new C()) { }
            await foreach (var x in new D()) { }
            //await foreach (var x in new E()) { } コメントを外すとエラーに
        }
    }
}
