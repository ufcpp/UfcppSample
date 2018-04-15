namespace OverloadResolution.ValueTypeConversion
{
    using System;

    struct S : IDisposable
    {
        public void Dispose() { }
    }

    class Program
    {
        static void Main()
        {
            // S は ValueType から派生しているかのように振る舞うものの、これはあくまで ValueType への型変換になる
            // インターフェイスへの変換と同列なので、以下の呼び出しは不明瞭
#if Uncompilable
            M(new S());
#endif
        }

        static void M(IDisposable x) => Console.WriteLine("IDisposable");
        static void M(ValueType x) => Console.WriteLine("ValueType");
    }
}
