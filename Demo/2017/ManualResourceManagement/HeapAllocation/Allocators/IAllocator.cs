namespace HeapAllocation.Allocators
{
    /// <summary>
    /// メモリ確保・解放用のインターフェイス。
    /// 特に汎用化するつもりはないので、<see cref="PointerPoint"/>専用。
    /// </summary>
    unsafe interface IAllocator
    {
        /// <summary>
        /// int 2つ分(8バイト)の領域を返してもらう。
        /// </summary>
        int* Alloc();

        /// <summary>
        /// <see cref="Alloc"/>で確保した領域を解放する。
        /// </summary>
        void Release(int* p);
    }
}
