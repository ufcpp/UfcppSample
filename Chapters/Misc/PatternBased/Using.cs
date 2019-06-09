namespace PatternBased.Using
{
    using System;

    struct Disposable
        // インターふぃえす実装が必須。
        // 以下の行をコメントアウトするとコンパイル エラーになる。
        : IDisposable
    {
        public void Dispose() { }
    }

    class Program
    {
        static void Main()
        {
            using (var d = new Disposable()) ;
        }
    }
}
