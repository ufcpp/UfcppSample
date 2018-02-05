using System;

namespace StringManipulation.SafeStackalloc
{
    public ref struct UpperCaseSplitter
    {
        private ReadOnlySpan<char> _p;
        public UpperCaseSplitter(string s) => _p = s;

        public bool TryMoveNext(out ReadOnlySpan<char> span)
        {
            var p = _p;

            if (p.Length == 0)
            {
                span = default;
                return false;
            }

            var i = 0;
            while (++i < p.Length && !char.IsUpper(p[i])) ;

            span = p.Slice(0, i);
            _p = p.Slice(i);
            return true;
        }
    }
}
