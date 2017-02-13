using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace HeapAllocation
{
    /// <summary>
    /// 遅いのわかってて、どのくらい遅いのかを示すための型。
    /// <see cref="Marshal.AllocHGlobal(int)"/>(かなり遅い)でヒープ確保・解放するとどうなるか例示するのに使う。
    /// </summary>
    unsafe struct PointHGlobal : IDisposable
    {
        private readonly int* _pointer;

        public ref int X => ref *_pointer;
        public ref int Y => ref _pointer[1];

        private PointHGlobal(int* p) => _pointer = p;

        public static PointHGlobal New(int x, int y)
        {
            var p = (int*)Marshal.AllocHGlobal(sizeof(int) * 2);
            *p = x;
            p[1] = y;
            return new PointHGlobal(p);
        }

        public void Dispose() => Marshal.Release((IntPtr)_pointer);
    }
}
