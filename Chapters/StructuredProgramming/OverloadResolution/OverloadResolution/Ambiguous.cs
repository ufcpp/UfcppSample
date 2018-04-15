namespace OverloadResolution.Ambiguous
{
    using System;

    // インターフェイス実装とユーザー定義の型変換を持つ
    class A : IDisposable
    {
        public void Dispose() { }
        public static implicit operator int(A x) => 0;
    }

    class Program
    {
        static void M(IDisposable x) => Console.WriteLine("IDisposable");
        static void M(int x) => Console.WriteLine("int");

        static void Main()
        {
            // インターフェイスへの変換と、ユーザー定義の型変換は同列
            // どちらを呼ぶべきか、このコードでは解決できない
#if Uncompilable
            M(new A());
#endif

            // 明示的にキャストを書けば大丈夫
            M((IDisposable)new A());
            M((int)new A());
        }
    }
}
