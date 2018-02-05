namespace StringManipulation.Unsafe
{
    /// <summary>
    /// 文字列を大文字になっている個所で文字列を分割。
    /// </summary>
    public struct UpperCaseSplitter : IStringSplitter
    {
        public unsafe bool TryMoveNext(ref StringSpan state, out StringSpan word)
        {
            var p = state.Pointer;
            var end = p + state.Length;

            if (p >= end)
            {
                word = default;
                return false;
            }

            while (++p < end && !char.IsUpper(*p)) ;

            var len = (int)(p - state.Pointer);
            word = state.Slice(0, len);
            state = state.Slice(len);

            return true;
        }
    }
}
