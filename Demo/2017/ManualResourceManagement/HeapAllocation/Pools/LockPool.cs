using System;
using System.Runtime.InteropServices;

namespace HeapAllocation.Pools
{
    /// <summary>
    /// lock を使ってスレッド安全にしたメモリ プール。
    /// </summary>
    unsafe struct LockPool : IAllocator, IDisposable
    {
        public static readonly LockPool Instance = new LockPool(20);

        private int _poolSize;
        private int* _pool;
        private object _lockObj;

        public LockPool(int poolSize)
        {
            _poolSize = poolSize;
            var p = Marshal.AllocHGlobal(poolSize * (sizeof(int) * 3));
            var ip = (int*)p;
            for (int i = 0; i < poolSize * 3; i += 3)
            {
                ip[i] = 0;
            }
            _pool = ip;

            _lockObj = new object();
        }

        public void Dispose() => Marshal.Release((IntPtr)_pool);

        public int* Alloc()
        {
            for (int i = 0; i < _poolSize * 3; i += 3)
            {
                lock (_lockObj)
                {
                    if (_pool[i] == 0)
                    {
                        _pool[i] = 1;
                        return &_pool[i + 1];
                    }
                }
            }
            throw new OutOfMemoryException();
        }

        public void Release(int* p)
        {
            lock(_lockObj)
            {
                p[-1] = 0;
            }
        }
    }
}
