namespace StringManipulation.Unsafe
{
    public unsafe ref struct Splitter
    {
        private char* _p;
        private readonly char* _end;
        private readonly char _delimiter;
        public Splitter(char* p, int length, char delimiter)
        {
            _p = p;
            _end = p + length;
            _delimiter = delimiter;
        }

        public bool TryMoveNext(out StringSpan segment)
        {
            var p = _p;
            var end = _end;

            if (p >= end)
            {
                segment = default;
                return false;
            }

            while (++p < end && *p != _delimiter) ;

            var len = (int)(p - _p);
            segment = new StringSpan(_p, len);

            _p = p + 1;

            return true;
        }
    }
}
