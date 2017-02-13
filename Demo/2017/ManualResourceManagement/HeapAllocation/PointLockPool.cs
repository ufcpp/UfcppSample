using System;
using System.Runtime.InteropServices;

namespace HeapAllocation
{
    /// <summary>
    /// 最初にある程度のサイズのヒープをプールして、自前管理でオブジェクトの払い出し・返却をする実装。
    /// プールしてる以外は<see cref="PointHGlobal"/>とほとんど同じ実装。
    /// lock を使って実装。
    /// </summary>
    unsafe struct PointLockPool : IDisposable
    {
        private readonly int* _pointer;

        public ref int X => ref *_pointer;
        public ref int Y => ref _pointer[1];

        private PointLockPool(int* p) => _pointer = p;

        public static PointLockPool New(int x, int y)
        {
            var p = Alloc();
            *p = x;
            p[1] = y;
            return new PointLockPool(p);
        }

        public void Dispose() => Release(_pointer);

        #region プール

        /// <summary>
        /// 何要素分ヒープ確保するか。
        /// 今回、かなり少な目にしてる。
        /// </summary>
        const int PoolSize = 20;

        private static bool* _usedFlags;
        private static int* _pool;
        private static object _lockObj = new object();

        static PointLockPool()
        {
            var p = Marshal.AllocHGlobal(PoolSize * (sizeof(int) * 2 + sizeof(bool)));
            _pool = (int*)p;
            _usedFlags = (bool*)(p + PoolSize * sizeof(int) * 2);
            for (int i = 0; i < PoolSize; i++)
            {
                _usedFlags[i] = false;
            }
        }

        private static int* Alloc()
        {
            for (int i = 0; i < PoolSize; i++)
            {
                lock (_lockObj)
                {
                    if (!_usedFlags[i])
                    {
                        _usedFlags[i] = true;
                        return &_pool[2 * i];
                    }
                }
            }
            throw new OutOfMemoryException();
        }

        private static void Release(int* p)
        {
            lock(_lockObj)
            {
                var i = (p - _pool) / 2;
                _usedFlags[i] = false;
            }
        }

        #endregion
    }
}
