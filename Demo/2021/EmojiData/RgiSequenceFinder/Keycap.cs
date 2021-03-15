using System;

namespace RgiSequenceFinder
{
    /// <summary>
    /// RGI emoji keycap sequence の1文字目。
    /// 参考: https://unicode.org/reports/tr51/#def_std_emoji_keycap_sequence_set
    /// </summary>
    /// <remarks>
    /// 絵文字シーケンスの中で唯一 ASCII 文字開始なたちの悪いシーケンスなので独立して判定。
    ///
    /// 絶対 ASCII 1文字なので素の char とか byte でもいいんだけど、範囲チェックとか ToString とかを足しとく。
    /// </remarks>
    public readonly struct Keycap
    {
        public readonly byte Value;
        private Keycap(char value) => Value = (byte)value;

        /// <summary>
        /// RGI emoji keycap sequence の時はそれの1文字目、
        /// そうでないときは 0 (ヌル文字)を返す。
        /// </summary>
        public static Keycap Create(ReadOnlySpan<char> s)
        {
            if (s.Length < 3) return default;

            // combining enclosing keycap
            if (s[2] != 0x20E3) return default;

            // variation selector-16
            if (s[1] != 0xFE0F) return default;

            return s[0] is (>= '0' and <= '9') or '#' or '*' ? new(s[0]) : default;

        }

        public override string ToString() => ((char)Value).ToString();
    }
}
