namespace UtfString.Utf8
{
    public struct ArrayAccessor
    {
        private readonly byte[] _data;

        public ArrayAccessor(byte[] data) { _data = data; }

        public byte this[int index] => _data[index];

        public int Length => _data.Length;
    }
}
