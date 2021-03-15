using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RgiSequenceFinder
{
    /// <summary>
    /// 「別途単体テスト プロジェクトを作れ」と言われるとぐうの音も出ないメソッド群。
    /// </summary>
    class Test
    {
        public static void Keycap()
        {
            var count = 0;
            foreach (var s in Data.RgiEmojiSequenceList)
            {
                if (GraphemeBreak.IsKeycap(s))
                {
                    ++count;
                    Console.WriteLine(s);
                }
            }
            Console.WriteLine(count);
        }

        public static void FlagSequence()
        {
            var count = 0;
            foreach (var s in Data.RgiEmojiSequenceList)
            {
                if (RegionalIndicator.Create(s) is { First: not 0 } code)
                {
                    ++count;
                    Console.WriteLine(code);
                }
            }
            Console.WriteLine(count);
        }

        public static void TagSequence()
        {
            var count = 0;
            foreach (var s in Data.RgiEmojiSequenceList)
            {
                var (tagCount, tags) = Tags.FromFlagSequence(s);
                if (tagCount > 0)
                {
                    ++count;

                    // 現行の RGI だと gbeng, gbsct, gbwls の3つだけのはず。
                    Console.WriteLine(tags);
                }
            }
            Console.WriteLine(count);
        }

        /// <summary>
        /// <see cref="Data.RgiEmojiSequenceList"/> に入れた文字は全部「最後まで1つなぎの絵文字シーケンス」判定を受けるはず。
        /// </summary>
        public static void Rgi絵文字シーケンス全体をGetEmojiSequenceLengthにかける()
        {
            foreach (var s in Data.RgiEmojiSequenceList)
            {
                var (type, length) = GraphemeBreak.GetEmojiSequenceLength(s);
                if (length != s.Length)
                {
                    Console.WriteLine("error0 " + s);
                }
                if (type == EmojiSequenceType.NotEmoji)
                {
                    Console.WriteLine("error1 " + s);
                }
            }
        }

        /// <summary>
        /// <see cref="Data.RgiEmojiSequenceList"/> 前後に無関係の文字を挟んでみて、ちゃんと絵文字シーケンスの部分だけ抜き出されてるか見てみる。
        /// </summary>
        public static void Rgi絵文字シーケンスの前後にAsciiを挟んでからGetEmojiSequenceLengthにかける()
        {
            const string NonEmoji = "abc";
            foreach (var s in Data.RgiEmojiSequenceList)
            {
                var s2 = NonEmoji + s + NonEmoji;
                (EmojiSequenceType type, int length) len;

                var span = s2.AsSpan();

                // 非絵文字部分、常に0が返ってきて1文字進めればいいはず。
                for (int i = 0; i < NonEmoji.Length; i++)
                {
                    len = GraphemeBreak.GetEmojiSequenceLength(span);
                    if (len.type != EmojiSequenceType.NotEmoji) Console.WriteLine("error1 " + s);
                    span = span.Slice(len.length);
                }

                // 絵文字部分、元の文字列と同じはず。
                len = GraphemeBreak.GetEmojiSequenceLength(span);
                if (len.type == EmojiSequenceType.NotEmoji) Console.WriteLine("error2 " + s);
                if (len.length != s.Length)
                {
                    Console.WriteLine("error3 " + s);
                }
                span = span.Slice(len.length);

                // 非絵文字部分、常に0が返ってきて1文字進めればいいはず。
                for (int i = 0; i < NonEmoji.Length; i++)
                {
                    len = GraphemeBreak.GetEmojiSequenceLength(span);
                    if (len.type != EmojiSequenceType.NotEmoji) Console.WriteLine("error4 " + s);
                    span = span.Slice(len.length);
                }

                // 最後まで読み切ったはず。
                if (span.Length != 0) Console.WriteLine("error5 " + s);
            }
        }
    }
}
