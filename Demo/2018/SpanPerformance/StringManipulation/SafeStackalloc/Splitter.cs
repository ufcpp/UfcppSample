using System;

namespace StringManipulation.SafeStackalloc
{
    public struct Splitter : IStringSplitter
    {
        private readonly char _delimiter;
        public Splitter(char delimiter) => _delimiter = delimiter;

        public unsafe bool TryMoveNext(ref ReadOnlySpan<char> state, out ReadOnlySpan<char> word)
        {
            var p = state;

            if (p.Length == 0)
            {
                word = default;
                return false;
            }

            var i = 0;
            while (++i < p.Length && p[i] != _delimiter) ;

            word = p.Slice(0, i);
            if (i != p.Length) ++i;
            state = p.Slice(i);

            return true;
        }
    }
}
