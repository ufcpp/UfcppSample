namespace UtfString.Unsafe.DualEncoding
{
    public struct ArrayAccessor
    {
        private bool _isWideChar;
        private readonly byte[] _data;

        public ArrayAccessor(bool isWideChar, byte[] data)
        {
            _isWideChar = isWideChar;
            _data = data;
        }

        public unsafe ushort this[int index]
        {
            get
            {
                if (_isWideChar)
                {
                    fixed (byte* pb = _data)
                    {
                        ushort* p = (ushort*)pb;
                        return p[index];
                    }
                }
                else return _data[index];
            }
        }

        public int Length => _isWideChar ? _data.Length / 2 : _data.Length;
    }
}
