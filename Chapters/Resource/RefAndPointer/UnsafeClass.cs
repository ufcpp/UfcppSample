namespace RefAndPointer.UnsafeClass
{
    using System;
    using System.Runtime.CompilerServices;

    class Program
    {
        static void Main()
        {
            unsafe
            {
                int x = 1;
                void* pointer = Unsafe.AsPointer(ref x);
                *(int*)pointer = 2;

                Console.WriteLine(x); // 2 になってる

                ref int r = ref Unsafe.AsRef<int>(pointer);
                r = 3;

                Console.WriteLine(*(int*)pointer); // 3 になってる
            }
        }
    }
}
