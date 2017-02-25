using System;
using System.Runtime.InteropServices;

namespace HeapAllocation.Allocators
{
    /// <summary>
    /// プールなし、普通に<see cref="Marshal.AllocHGlobal(int)"/>を都度呼ぶアロケーター。
    /// どうしようもなく遅い。
    /// </summary>
    unsafe struct MallocAllocator : IAllocator
    {
        public static readonly MallocAllocator Instance = new MallocAllocator();

        public int* Alloc() => (int*)Interop.malloc(sizeof(int) * 2);
        public void Release(int* p) => Interop.free((IntPtr)p);
    }
}
