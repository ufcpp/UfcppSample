using System;
public static class StringExtensions
{
    /// <summary>
    /// 結局、これが有力。
    /// stackalloc した領域に直接エンコード結果を書き込む。
    /// UTF16とUTF8の性質上、string の UTF8 エンコード結果の byte 数は、元の<see cref="string.Length"/>の3倍の長さ以上にはならない。
    /// なので、GetByteCount 的なものは実装せず、3倍長さで stackalloc してしまう想定。
    /// </summary>
    public static int GetBytes(this string s, Span<byte> buf)
    {
        int j = 0;
        for (int i = 0; i < s.Length; i++)
        {
            var c = s[i];

            if (c < 0x80)
            {
                buf[j++] = (byte)c;
            }
            else if (c < 0b1000_0000_0000)
            {
                buf[j++] = (byte)(0b11000000 | (c >> 6) & 0b1_1111);
                buf[j++] = (byte)(0b10000000 | c & 0b11_1111);
            }
            else if (!char.IsHighSurrogate(c))
            {
                buf[j++] = (byte)(0b11100000 | (c >> 12) & 0b1111);
                buf[j++] = (byte)(0b10000000 | (c >> 6) & 0b11_1111);
                buf[j++] = (byte)(0b10000000 | c & 0b11_1111);
            }
            else
            {
                var cp = (uint)(c & 0b0011_1111_1111) + 0b0100_0000;

                ++i;
                if (i == s.Length) return i;

                cp = (cp << 10) | ((uint)s[i] & 0b0011_1111_1111);

                buf[j++] = (byte)(0b11110000 | (cp >> 18) & 0b111);
                buf[j++] = (byte)(0b10000000 | (cp >> 12) & 0b11_1111);
                buf[j++] = (byte)(0b10000000 | (cp >> 6) & 0b11_1111);
                buf[j++] = (byte)(0b10000000 | cp & 0b11_1111);
            }
        }
        return j;
    }
}
