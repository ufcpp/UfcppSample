namespace FastEnumeration
{
    /// <summary>
    /// <see cref="NormalEnumerator"/> と同じことを <see cref="IFastEnumerator{T}"/> で実装。
    /// </summary>
    class FastEnumerator : IFastEnumerator<int>
    {
        private readonly int[] _data;
        public FastEnumerator(int[] data) => _data = data;

        private int _i = -1;

        public int TryMoveNext(out bool success)
        {
            var i = ++_i;
            var data = _data;
            if ((uint)i < (uint)data.Length)
            {
                success = true;
                return data[i];
            }
            else
            {
                success = false;
                return default;
            }
        }
    }
}
