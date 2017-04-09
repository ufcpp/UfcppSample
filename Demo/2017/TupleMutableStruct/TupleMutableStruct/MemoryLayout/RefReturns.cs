namespace TupleMutableStruct.MemoryLayout.RefReturns
{
    using static System.Console;
    using System.Runtime.CompilerServices;

    class Program
    {
        static void Main()
        {
            var v = new Vector { A = 1, B = 2, C = 3, D = 4, E = 5, F = 6, G = 7, H = 8 };

            // これまでも書けたが、ポインターが必要だった
            unsafe
            {
                long* p = (long*)(void*)&v;
                WriteLine(p->ToString("X"));
            }

            // 参照戻り値があれば、ポインターなしで同様のことができる
            // ここには unsafe 宣言が不要
            ref long r = ref AsLong(ref v);
            WriteLine(r.ToString("X"));
        }

        // Unsafe クラスを利用
        // これ自身は、クラス名の通り unsafe だし、ポインターを介する
        unsafe static ref long AsLong(ref Vector v)
            => ref Unsafe.AsRef<long>(Unsafe.AsPointer(ref v));
    }

}
