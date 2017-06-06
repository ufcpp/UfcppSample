namespace BufferPerformance.Buffers
{
    unsafe struct PointerSpan : IByteSpan
    {
        byte* _pointer;
        int _length;
        public PointerSpan(byte* pointer, int length)
        {
            _pointer = pointer;
            _length = length;
        }
        public ref byte this[int index] => ref _pointer[index];
        public int Length => _length;
    }
}
