using System;

namespace StringManipulation.Unsafe
{
    /// <summary>
    /// 先頭だけ <see cref="char.ToUpper(char)"/>。
    /// </summary>
    public struct ToCamel : IStringFormatter
    {
        public unsafe void Write(StringSpan word, ref StringSpan buffer)
        {
            *buffer.Pointer = char.ToUpper(*word.Pointer);
            var size = sizeof(char) * (word.Length - 1);
            Buffer.MemoryCopy(word.Pointer + 1, buffer.Pointer + 1, size, size);
            buffer = buffer.Slice(word.Length);
        }
    }
}
