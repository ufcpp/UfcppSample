namespace Preview2.EnhancedUsing
{
    using System;

    readonly ref struct DeferredMessage
    {
        private readonly string _message;
        public DeferredMessage(string message) => _message = message;

        // using で使うのに、IDisposable インターフェイス実装が要らない。
        // ただ、既存コードを壊さないようにするためには、ref struct の場合限定らしい。
        // 普通のクラス・構造体の場合、IDisposable インターフェイス実装が必須。
        public void Dispose() => Console.WriteLine(_message);
    }

    class Program
    {
        static void Main()
        {
            // using var で、変数のスコープに紐づいた using になる。
            // スコープを抜けるときに Dispose が呼ばれる。
            using var a = new DeferredMessage("a");
            using var b = new DeferredMessage("b");

            Console.WriteLine("c");

            // c, b, a の順でメッセージが表示される
        }

        // Preview 2 ではまだみたいだけど、
        // C# 8.0 リリース版までには、foreach も pattern-based なものが認められるようになるはず。
        // foreach は最後に Dispose を呼ぶ仕様になってるけども、それも、pattern-based に。
    }
}
