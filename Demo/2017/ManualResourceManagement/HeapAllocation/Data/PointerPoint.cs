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
        public static PointerPoint New<Allocator>(this Allocator pool, int x, int y)
            where Allocator : Allocators.IAllocator
        {
            var p = pool.Alloc();
            p[0] = x;
            p[1] = y;
            return new PointerPoint(p);
        }

        public static void Delete<Allocator>(this Allocator pool, PointerPoint p)
            where Allocator : Allocators.IAllocator
        {
            pool.Release(p._pointer);
        }
    }
}
