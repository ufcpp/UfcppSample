using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace HeapAllocation
{
    /// <summary>
    /// <see cref="PointLockPool"/>と基本的に同じ。
    /// lock をやめて、CAS (Compare And Swap。<see cref="Interlocked.CompareExchange(ref int, int, int)"/>)を使うようにしたもの。
    /// lock よりはだいぶ速いはず。
    /// </summary>
    unsafe struct PointCasPool : IDisposable
    {
        private readonly int* _pointer;

        public ref int X => ref *_pointer;
        public ref int Y => ref _pointer[1];

        private PointCasPool(int* p) => _pointer = p;

        public static PointCasPool New(int x, int y)
        {
            var p = Alloc();
            *p = x;
            p[1] = y;
            return new PointCasPool(p);
        }

        public void Dispose() => Release(_pointer);

        #region プール

        /// <summary>
        /// 何要素分ヒープ確保するか。
        /// 今回、かなり少な目にしてる。
        /// </summary>
        const int PoolSize = 20;

        private static int* _usedFlags;
        private static int* _pool;
        private static object _lockObj = new object();

        static PointCasPool()
        {
            var p = Marshal.AllocHGlobal(PoolSize * (sizeof(int) * 3));
            _pool = (int*)p;
            _usedFlags = (int*)(p + PoolSize * sizeof(int) * 2);
            for (int i = 0; i < PoolSize; i++)
            {
                _usedFlags[i] = 0;
            }
        }

        private static int* Alloc()
        {
            for (int i = 0; i < PoolSize; i++)
            {
                if(Interlocked.CompareExchange(ref _usedFlags[i], 1, 0) == 0)
                {
                    return &_pool[2 * i];
                }
            }
            throw new OutOfMemoryException();
        }

        private static void Release(int* p)
        {
            lock(_lockObj)
            {
                var i = (p - _pool) / 2;
                Interlocked.Exchange(ref _usedFlags[i], 0);
            }
        }

        #endregion
    }
}
