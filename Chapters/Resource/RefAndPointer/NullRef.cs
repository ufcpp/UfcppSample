namespace RefAndPointer._NullRef
{
    using System;
    using System.Runtime.CompilerServices;

    unsafe static class NullReference
    {
        public static ref T Null<T>() => ref Unsafe.AsRef<T>((void*)0);
        public static bool IsNull<T>(ref T x) => Unsafe.AsPointer(ref x) == (void*)0;
    }

    class Program
    {
        static void Main()
        {
            ref var x = ref NullReference.Null<int>();
            Console.WriteLine(NullReference.IsNull(ref x)); // true
            Console.WriteLine(x); // 実行時エラー。NullReferenceException 発生
        }
    }
}
