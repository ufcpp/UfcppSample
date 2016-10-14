namespace UtfString.Generic
{
    public interface IArrayAccessor<TChar>
        where TChar : struct
    {
        TChar this[int index] { get; }
        int Length { get; }
    }

    public struct ByteAccessor : IArrayAccessor<byte>
    {
        private readonly byte[] _data;
        public ByteAccessor(byte[] data) { _data = data; }
        public byte this[int index] => _data[index];
        public int Length => _data.Length;

        public static implicit operator ByteAccessor(byte[] data) => new ByteAccessor(data);
    }

    public struct ShortAccessor : IArrayAccessor<ushort>
    {
        private readonly byte[] _data;

        public ShortAccessor(byte[] data) { _data = data; }

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

        public static implicit operator ShortAccessor(byte[] data) => new ShortAccessor(data);
    }

    public struct IntAccessor : IArrayAccessor<uint>
    {
        private readonly byte[] _data;

        public IntAccessor(byte[] data) { _data = data; }

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

        public static implicit operator IntAccessor(byte[] data) => new IntAccessor(data);
    }

    public struct DualAccessor : IArrayAccessor<ushort>
    {
        private bool _isWideChar;
        private readonly byte[] _data;

        public DualAccessor(bool isWideChar, byte[] data)
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

        public static implicit operator DualAccessor((bool isWideChar, byte[] data) x) => new DualAccessor(x.isWideChar, x.data);
        public static implicit operator DualAccessor(byte[] data) => new DualAccessor(true, data);
    }
}
