namespace Preview2.PatternBasedUsing
{
    using System;

    // これまで通り、using で使える型。
    struct Disposable : IDisposable
    {
        public void Dispose() { }
    }

    // 残念ながら IDisposable を実装していないと using で使えない。
    struct NonDisposable
    {
        public void Dispose() { }
    }

    // となると、インターフェイスを実装できない ref struct で困っていた。
    // ref struct の場合、IDisposable なしでも Dispose メソッドさえあれば using で使えるようになった。
    ref struct RefDisposable
    {
        public void Dispose() { }
    }

    class Program
    {
        static void Main()
        {
            // この行は元々 OK。
            using (new Disposable()) { }

#if false
            // 残念ながら今でもコンパイル エラーに。
            using (new NonDisposable()) { }
#endif

            // C# 8.0 で、これは OK になった。
            using (new RefDisposable()) { }
        }
    }
}
