using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ConsoleApp1._04_Ref
{
    // ガベコレ管理対象外のヒープを使いたい
    // 自前でメモリ管理をしたい場面、一般のアプリではほとんどないけど、native相互運用とかしだすとあり得る
    unsafe struct UnmanagedReference<T> : IDisposable
        where T : struct
    {
        void* _pointer;

        public static UnmanagedReference<T> New() => new UnmanagedReference<T>(Size);
        static readonly int Size = Unsafe.SizeOf<T>();
        private UnmanagedReference(int size) => _pointer = (void*)Marshal.AllocHGlobal(size);

        public ref T Value => ref Unsafe.AsRef<T>(_pointer);

        public void Dispose() => Marshal.FreeHGlobal((IntPtr)_pointer);
    }

    struct Vector3
    {
        public int X;
        public int Y;
        public int Z;
    }

    class _04_Unsafe
    {
        public static void Run()
        {
            using (var r = UnmanagedReference<Vector3>.New())
            {
                ref var v = ref r.Value;

                v.X = 1;
                v.Y = 1;
                v.Z = 1;
            }
        }
    }
}
