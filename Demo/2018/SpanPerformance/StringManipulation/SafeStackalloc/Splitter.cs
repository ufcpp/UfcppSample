using System;

namespace StringManipulation.SafeStackalloc
{
    public ref struct Splitter
    {
        private ReadOnlySpan<char> _p;
        private readonly char _delimiter;
        public Splitter(string s, char delimiter)
        {
            _p = s;
            _delimiter = delimiter;
        }

        public bool TryMoveNext(out ReadOnlySpan<char> span)
        {
            var p = _p;

            if (p.Length == 0)
            {
                span = default;
                return false;
            }

            var i = 0;
            while (++i < p.Length && p[i] != _delimiter) ;

            span = p.Slice(0, i);

            if (i == p.Length) _p = default;
            else _p = p.Slice(i + 1);

            return true;
        }
    }
}
