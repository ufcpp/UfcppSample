namespace StringManipulation.Unsafe
{
    public struct Splitter : IStringSplitter
    {
        private readonly char _delimiter;
        public Splitter(char delimiter) => _delimiter = delimiter;

        public unsafe bool TryMoveNext(ref StringSpan state, out StringSpan word)
        {
            var p = state.Pointer;
            var end = p + state.Length;

            if (p >= end)
            {
                word = default;
                return false;
            }

            while (++p < end && *p != _delimiter) ;

            var len = (int)(p - state.Pointer);
            word = state.Slice(0, len);
            if (len != state.Length) ++len;
            state = state.Slice(len);

            return true;
        }
    }
}
