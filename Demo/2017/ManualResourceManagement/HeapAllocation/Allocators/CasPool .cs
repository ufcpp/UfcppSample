using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace HeapAllocation.Allocators
{
    /// <summary>
    /// lock をやめて、CAS (Compare And Swap。<see cref="Interlocked.CompareExchange(ref int, int, int)"/>)を使うようにしたメモリ プール。
    /// lock よりはだいぶ速いはず。
    /// </summary>
    unsafe struct CasPool : IAllocator, IDisposable
    {
        public static readonly CasPool Instance = new CasPool(20);

        private int _poolSize;
        private int* _pool;

        public CasPool(int poolSize)
        {
            _poolSize = poolSize;
            var p = Interop.malloc(poolSize * (sizeof(int) * 3));
            var ip = (int*)p;
            for (int i = 0; i < poolSize * 3; i += 3)
            {
                ip[i] = 0;
            }
            _pool = ip;
        }

        public void Dispose() => Interop.free((IntPtr)_pool);

        public int* Alloc()
        {
            for (int i = 0; i < _poolSize * 3; i += 3)
            {
                if (Interlocked.CompareExchange(ref _pool[i], 1, 0) == 0)
                {
                    return &_pool[i + 1];
                }
            }
            throw new OutOfMemoryException();
        }

        public void Release(int* p)
        {
            Interlocked.Exchange(ref p[-1], 0);
        }
    }
}
