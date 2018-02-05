using System;

namespace StringManipulation.Unsafe
{
    /// <summary>
    /// 先頭だけ <see cref="char.ToLower(char)"/>。
    /// 最初の1個以外は先頭に _ を付け加える。
    /// </summary>
    public struct ToSnake : IStringFormatter
    {
        bool _nonFirst;
        public unsafe void Write(StringSpan word, ref StringSpan buffer)
        {
            var len = word.Length;
            var buf = buffer.Pointer;
            if (_nonFirst)
            {
                *buf++ = '_';
                ++len;
            }
            else _nonFirst = true;

            var p = word.Pointer;
            *buf++ = char.ToLower(*p);
            var size = sizeof(char) * (word.Length - 1);
            Buffer.MemoryCopy(p + 1, buf, size, size);
            buffer = buffer.Slice(len);
        }
    }
}
