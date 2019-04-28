namespace Preview2.PatternBasedUsingForeach
{
    using System;

    // GetEnumerator/MoveNext/Current は元々パターン ベース。
    // ただ、Dispose の呼び出しだけは IDisposable の実装が必須だった。
    // C# 8.0 で、ref struct の場合はパターン ベースで Dispose メソッドを呼んでもらえるように。
    ref struct RefEnumerable
    {
        public RefEnumerable GetEnumerator() => this;
        public int Current => 0;
        public bool MoveNext() => false;
        public void Dispose() => Console.WriteLine("ref disposed");
    }

    // RefEnumerable と比べて、 ref を取っただけ。
    struct BrokenEnumerable
    {
        public BrokenEnumerable GetEnumerator() => this;
        public int Current => 0;
        public bool MoveNext() => false;

        // この Dispose は呼ばれない。
        // ref struct でない場合、IDisposable インターフェイスの実装が必須。
        public void Dispose() => Console.WriteLine("broken disposed");
    }

    class Program
    {
        static void Main()
        {
            // ref disposed は表示される。
            foreach (var _ in new RefEnumerable()) ;

            // broken disposed は表示されない。
            // コンパイル エラーにはならないので特に注意。
            foreach (var _ in new BrokenEnumerable()) ;
        }
    }
}
