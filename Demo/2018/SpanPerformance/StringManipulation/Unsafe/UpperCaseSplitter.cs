namespace StringManipulation.Unsafe
{
    public unsafe ref struct UpperCaseSplitter
    {
        private char* _p;
        private readonly char* _end;
        public UpperCaseSplitter(char* p, int length)
        {
            _p = p;
            _end = p + length;
        }

        public bool TryMoveNext(out StringSegment segment)
        {
            var p = _p;
            var end = _end;

            if (p >= end)
            {
                segment = default;
                return false;
            }

            while (++p < end && !char.IsUpper(*p)) ;

            var len = (int)(p - _p);
            segment = new StringSegment(_p, len);

            _p = p;

            return true;
        }
    }
}
