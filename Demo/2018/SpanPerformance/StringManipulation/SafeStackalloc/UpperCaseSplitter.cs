using System;

namespace StringManipulation.SafeStackalloc
{
    /// <summary>
    /// 文字列を大文字になっている個所で文字列を分割。
    /// </summary>
    public struct UpperCaseSplitter : IStringSplitter
    {
        public unsafe bool TryMoveNext(ref ReadOnlySpan<char> state, out ReadOnlySpan<char> word)
        {
            var p = state;

            if (p.Length == 0)
            {
                word = default;
                return false;
            }

            var i = 0;
            while (++i < p.Length && !char.IsUpper(p[i])) ;

            word = p.Slice(0, i);
            state = p.Slice(i);

            return true;
        }
    }
}
