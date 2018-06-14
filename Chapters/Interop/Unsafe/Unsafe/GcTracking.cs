namespace Unsafe.GcTracking
{
    using System;
    using System.Runtime.CompilerServices;
    using static System.Console;

    // C# の参照型が内部的にどうなっているか試してみるために、フィールド1個だけのクラスを用意。
    class X
    {
        // フィールドが1個だけなので、順序に悩む必要なし。
        // クラスの場合、フィールドが複数あるとき、並び順はコンパイラーが自由に変えていい仕様になってるので注意。
        // (StructLayout 属性を付けて制御はできる。)
        public int Value;
    }

    unsafe class Program
    {
        // 参照型変数が指す先のヒープのアドレスを取得。
        // Unsafe クラスは、C# では絶対に書けない処理をやってくれる(中身は IL assebler 実装)。
        // C# の unsafe コード以上に unsafe なことができるやべーやつ。
        // IL は案外がばがばで、C# コンパイラーのレベルで安全性を保証してることが結構ある。
        static ulong AsUnmanaged<T>(T r) where T : class => (ulong)Unsafe.As<T, IntPtr>(ref r);

        // 同上、ref が指す先のアドレスを取得。
        static ulong AsUnmanaged<T>(ref T r) => (ulong)Unsafe.AsPointer(ref r);
        
        static void Main()
        {
            // GC 誘発用に無駄オブジェクトを無駄に大量生成。
            void GenerageGarbage()
            {
                for (int i = 0; i < 1000000; i++) { var dummy = new object(); }
            }

            GenerageGarbage();

            var x = new X { Value = 12345678 };
            ref var r = ref x.Value;

            // 通常ではない手段(Unsafe クラス)を使って、managed ポインターを無理やり unmanaged ポインター化。
            var addressOfX = AsUnmanaged(x);
            var addressOfValue = AsUnmanaged(ref r);

            WriteLine((addressOfX, addressOfValue));

            GenerageGarbage();
            GC.Collect(0, GCCollectionMode.Forced);
            WriteLine("--- ここで GC 発生 ---");

            // 無理やり数値化した方のアドレスまでは追えないので、当然、前のアドレスのまま。
            // もう無効なアドレスなので、ここに対して読み書きするとクラッシュ・セキュリティ ホールの原因になる。
            WriteLine("unmanaged " + (addressOfX, addressOfValue));

            // GC 発生後、アドレスが変わってる。
            // 大体は前に移動しているはずなので、値が小さくなってる。
            WriteLine("managed   " + (AsUnmanaged(x), AsUnmanaged(ref r)));

            fixed (int* p = &x.Value)
            {
                // fixed している間はどれだけゴミを出そうが x は移動しない。
                GenerageGarbage();
                GC.Collect(0, GCCollectionMode.Forced);
                WriteLine("--- ここで GC 発生(fixed) ---");

                // fixe 直前と変わってないはず。
                WriteLine("managed   " + (AsUnmanaged(x), AsUnmanaged(ref r)));
            }
        }
    }
}
