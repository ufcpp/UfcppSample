using System;

namespace StringManipulation.SafeStackalloc
{
    /// <summary>
    /// <see cref="Unsafe.ToCamel"/>
    /// </summary>
    public struct ToCamel : IStringFormatter
    {
        public unsafe void Write(ReadOnlySpan<char> word, ref Span<char> buffer)
        {
            buffer[0] = char.ToUpper(word[0]);
            word.Slice(1).CopyTo(buffer.Slice(1));
            buffer = buffer.Slice(word.Length);
        }
    }
}
