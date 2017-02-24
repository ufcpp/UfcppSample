namespace HeapAllocation.Data
{
    /// <summary>
    /// GC を起こさないようにメモリ プール管理するとどうなるかという例。
    /// </summary>
    unsafe struct PointerPoint
    {
        internal readonly int* _pointer;

        public ref int X => ref *_pointer;
        public ref int Y => ref _pointer[1];

        internal PointerPoint(int* p) => _pointer = p;
    }

    unsafe static class PoolExtensions
    {
        public static PointerPoint New<TPool>(this TPool pool, int x, int y)
            where TPool : Pools.IAllocator
        {
            var p = pool.Alloc();
            p[0] = x;
            p[1] = y;
            return new PointerPoint(p);
        }

        public static void Delete<TPool>(this TPool pool, PointerPoint p)
            where TPool : Pools.IAllocator
        {
            pool.Release(p._pointer);
        }
    }
}
