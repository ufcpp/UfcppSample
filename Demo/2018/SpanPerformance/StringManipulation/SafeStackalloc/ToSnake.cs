using System;

namespace StringManipulation.SafeStackalloc
{
    /// <summary>
    /// <see cref="Unsafe.ToSnake"/>
    /// </summary>
    public struct ToSnake : IStringFormatter
    {
        bool _nonFirst;
        public unsafe void Write(ReadOnlySpan<char> word, ref Span<char> buffer)
        {
            if (_nonFirst)
            {
                buffer[0] = '_';
                buffer = buffer.Slice(1);
            }
            else _nonFirst = true;

            buffer[0] = char.ToLower(word[0]);
            word.Slice(1).CopyTo(buffer.Slice(1));
            buffer = buffer.Slice(word.Length);
        }
    }
}
