namespace RefAndPointer.UnsafeRef
{
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    struct ManagedBuffer
    {
        int[] _array;
        public ManagedBuffer(int length) { _array = new int[length]; }

        public ref int this[int index] => ref _array[index];
    }

    unsafe struct UnsafeBuffer
    {
        void* _pointer;
        public UnsafeBuffer(int* pointer) { _pointer = pointer; }

        public ref int this[int index] => ref Unsafe.AsRef<int>(_pointer);
    }

    class Program
    {
        unsafe static void Main()
        {
            // 配列と
            var b1 = new ManagedBuffer(10);
            b1[0] = 1;

            // スタック領域と
            var stack = stackalloc int[10];
            var b2 = new UnsafeBuffer(stack);
            b2[0] = 1;

            // アンマネージなメモリとを同じように触れる
            var p = Marshal.AllocHGlobal(10 * sizeof(int));
            var b3 = new UnsafeBuffer((int*)p);
            b3[0] = 1;

            Marshal.Release(p);
        }
    }
}
