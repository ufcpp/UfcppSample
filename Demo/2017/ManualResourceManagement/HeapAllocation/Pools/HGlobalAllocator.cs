using System;
using System.Runtime.InteropServices;

namespace HeapAllocation.Pools
{
    /// <summary>
    /// プールなし、普通に<see cref="Marshal.AllocHGlobal(int)"/>を都度呼ぶアロケーター。
    /// どうしようもなく遅い。
    /// </summary>
    unsafe struct HGlobalAllocator : IAllocator
    {
        public static readonly HGlobalAllocator Instance = new HGlobalAllocator();

        public int* Alloc() => (int*)Marshal.AllocHGlobal(sizeof(int) * 2);
        public void Release(int* p) => Marshal.Release((IntPtr)p);
    }
}
