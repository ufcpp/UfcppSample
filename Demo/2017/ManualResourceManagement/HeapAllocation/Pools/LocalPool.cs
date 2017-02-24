using System;
using System.Runtime.InteropServices;

namespace HeapAllocation.Pools
{
    /// <summary>
    /// スレッドごと個別のプールを持つ前提で、同時実行制御を一切しないメモリ プール。
    /// マルチスレッド実行しないならこのプールを1個作って使うとかやると、他のプール実装よりはだいぶ速い。
    /// </summary>
    /// <remarks>
    /// 性質上、static には管理できない。
    ///
    /// static がダメならスレッド ローカルにとか試みても、
    /// <see cref="System.Threading.ThreadLocal{T}.Value"/>取り出しが結構重たくてそんなに性能出ない。
    /// </remarks>
    unsafe struct LocalPool : IAllocator, IDisposable
    {
        private int _poolSize;
        private int* _pool;

        public LocalPool(int poolSize)
        {
            _poolSize = poolSize;
            var p = Marshal.AllocHGlobal(poolSize * (sizeof(int) * 3));
            var ip = (int*)p;
            for (int i = 0; i < poolSize * 3; i += 3)
            {
                ip[i] = 0;
            }
            _pool = ip;
        }

        public void Dispose() => Marshal.Release((IntPtr)_pool);

        public int* Alloc()
        {
            for (int i = 0; i < _poolSize * 3; i += 3)
            {
                if (_pool[i] == 0)
                {
                    _pool[i] = 1;
                    return &_pool[i + 1];
                }
            }
            throw new OutOfMemoryException();
        }

        public void Release(int* p)
        {
            p[-1] = 0;
        }
    }
}
