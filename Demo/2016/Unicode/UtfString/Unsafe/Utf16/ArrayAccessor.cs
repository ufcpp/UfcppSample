namespace UtfString.Unsafe.Utf16
{
    public struct ArrayAccessor
    {
        private readonly byte[] _data;

        public ArrayAccessor(byte[] data) { _data = data; }

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
}
