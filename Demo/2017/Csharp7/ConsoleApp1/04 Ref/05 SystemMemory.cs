using System;

namespace ConsoleApp1._04_Ref
{
    // System.Memory パッケージ内に Span っていう構造体があって
    // こいつを使うと、
    // - (managed な)配列
    // - ネイティブ ヒープ上のメモリ領域
    // - スタック(stackalloc したメモリ領域)
    // などについて、共通処理が書ける。
    class _05_SystemMemory
    {
        public unsafe static void Run()
        {
            ArraySpan();
            MallocSpan();
            StackallocSpan();
        }

        const int Length = 4;

        // Span に対する値の書き込み
        private static void SetValues(Span<int> s)
        {
            for (int i = 0; i < Length; i++)
            {
                // Span のインデクサーは ref 戻り値になってる
                ref var x = ref s[i];
                // Span が参照する先(配列 or ネイティブ ヒープ or スタック領域)が書き換わる
                x = i;
            }
        }

        // 配列からの Span 作成
        private static unsafe void ArraySpan()
        {
            var a = new int[Length];
            SetValues(new Span<int>(a));

            for (int i = 0; i < Length; i++)
            {
                Console.WriteLine(a[i]);
            }
        }

        // ネイティブ ヒープからの Span 作成
        private static unsafe void MallocSpan()
        {
            var p = (int*)Interop.malloc(sizeof(int) * Length);
            SetValues(new Span<int>((void*)p, Length));

            for (int i = 0; i < Length; i++)
            {
                Console.WriteLine(p[i]);
            }

            Interop.free((IntPtr)p);
        }

        // スタックからの Span 作成
        private static unsafe void StackallocSpan()
        {
            var p = stackalloc int[Length];
            SetValues(new Span<int>(p, Length));

            for (int i = 0; i < Length; i++)
            {
                Console.WriteLine(p[i]);
            }
        }
    }
}
