using System.Runtime.CompilerServices;

namespace UnsafeRefReturns.ValueNull
{
    class Program
    {
        unsafe static ref T NullRef<T>()
            // こういう型制約が必要かも
            //where T : blittable
            => ref Unsafe.AsRef<T>((void*)0);

        unsafe static void Main()
        {
            ref int x = ref NullRef<int>();

            x = 10; // int への書き込みなのにぬるぽ
        }
    }
}
