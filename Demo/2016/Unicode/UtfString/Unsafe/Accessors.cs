namespace UtfString.Unsafe
{
    public struct UShortAccessor
    {
        private readonly byte[] _data;

        public UShortAccessor(byte[] data) { _data = data; }

        public unsafe ushort this[int index]
        {
            get
            {
                fixed (byte* pb = _data)
                {
                    ushort* p = (ushort*)pb;
                    return p[index];
                }
            }
        }

        public int Length => _data.Length / 2;
    }

    public struct UIntAccessor
    {
        private readonly byte[] _data;

        public UIntAccessor(byte[] data) { _data = data; }

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
