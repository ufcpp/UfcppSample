namespace UtfString.Utf32
{
    public struct ArrayAccessor
    {
        private readonly byte[] _data;

        public ArrayAccessor(byte[] data) { _data = data; }

        public unsafe uint this[int index]
        {
            get
            {
                fixed (byte* pb = _data)
                {
                    uint* p = (uint*)pb;
                    return p[index];
                }
            }
        }

        public int Length => _data.Length / 4;
    }
}
