using System;

namespace RgiSequenceFinder
{
    /// <summary>
    /// 「別途単体テスト プロジェクトを作れ」と言われるとぐうの音も出ないメソッド群。
    /// </summary>
    class Test
    {
        public static void TestKeycap()
        {
            var count = 0;
            foreach (var s in Data.RgiEmojiSequenceList)
            {
                if (Keycap.Create(s) is { Value: not 0 } key)
                {
                    ++count;
                    Console.WriteLine(key);
                }
            }
            Console.WriteLine(count);
        }

        public static void TestFlagSequence()
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

        public static void TestTagSequence()
        {
            var count = 0;
            foreach (var s in Data.RgiEmojiSequenceList)
            {
                var (tagCount, tags) = TagSequence.FromFlagSequence(s);
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
                var (type, length) = GraphemeBreak.GetEmojiSequence(s);
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
                EmojiSequence len;

                var span = s2.AsSpan();

                // 非絵文字部分、常に0が返ってきて1文字進めればいいはず。
                for (int i = 0; i < NonEmoji.Length; i++)
                {
                    len = GraphemeBreak.GetEmojiSequence(span);
                    if (len.Type != EmojiSequenceType.NotEmoji) Console.WriteLine("error1 " + s);
                    span = span.Slice(len.LengthInUtf16);
                }

                // 絵文字部分、元の文字列と同じはず。
                len = GraphemeBreak.GetEmojiSequence(span);
                if (len.Type == EmojiSequenceType.NotEmoji) Console.WriteLine("error2 " + s);
                if (len.LengthInUtf16 != s.Length)
                {
                    Console.WriteLine("error3 " + s);
                }
                span = span.Slice(len.LengthInUtf16);

                // 非絵文字部分、常に0が返ってきて1文字進めればいいはず。
                for (int i = 0; i < NonEmoji.Length; i++)
                {
                    len = GraphemeBreak.GetEmojiSequence(span);
                    if (len.Type != EmojiSequenceType.NotEmoji) Console.WriteLine("error4 " + s);
                    span = span.Slice(len.LengthInUtf16);
                }

                // 最後まで読み切ったはず。
                if (span.Length != 0) Console.WriteLine("error5 " + s);
            }
        }
    }
}
