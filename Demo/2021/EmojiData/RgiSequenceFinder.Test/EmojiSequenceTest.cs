using System;
using System.Collections.Generic;
using Xunit;

namespace RgiSequenceFinder.Test
{
    public class EmojiSequenceTest
    {
        [Fact]
        public void TestKeycap()
        {
            var keys = new HashSet<byte>(new[] { (byte)'*', (byte)'#', (byte)'0', (byte)'1', (byte)'2', (byte)'3', (byte)'4', (byte)'5', (byte)'6', (byte)'7', (byte)'8', (byte)'9', });

            var count = 0;
            foreach (var s in Data.RgiEmojiSequenceList)
            {
                if (Keycap.Create(s) is { Value: not 0 } key)
                {
                    ++count;
                    Assert.Contains(key.Value, keys);
                }
            }

            Assert.Equal(12, count);
        }

        [Fact]
        public void TestFlagSequence()
        {
            var count = 0;
            foreach (var s in Data.RgiEmojiSequenceList)
            {
                if (RegionalIndicator.Create(s) is { First: not 0 } r)
                {
                    ++count;
                    Assert.True(r.First is >= (byte)'A' and <= (byte)'Z');
                    Assert.True(r.Second is >= (byte)'A' and <= (byte)'Z');
                }
            }

            // Regional Indicator を使った文字がこれ以上増えるとは思えないけど、一応バージョンによって変わる可能性はあるので注意。
            // 少なくとも Emoji 2.0 (Unicode 6.0) ～ Unicode 13.1 では258文字のはず。
            Assert.Equal(258, count);
        }

        [Fact]
        public void TestTagSequence()
        {
            // 現行の RGI だと gbeng, gbsct, gbwls の3つだけのはず。
            var subdivitions = new HashSet<string>(new[] { "gbeng", "gbsct", "gbwls" });

            var count = 0;
            foreach (var s in Data.RgiEmojiSequenceList)
            {
                var (tagCount, tags) = TagSequence.FromFlagSequence(s);
                if (tagCount > 0)
                {
                    ++count;

                    Assert.Contains(tags.ToString(), subdivitions);
                }
            }

            Assert.Equal(subdivitions.Count, count);
        }

        /// <summary>
        /// <see cref="Data.RgiEmojiSequenceList"/> に入れた文字は全部「最後まで1つなぎの絵文字シーケンス」判定を受けるはず。
        /// </summary>
        [Fact]
        public void Rgi絵文字シーケンス全体をGetEmojiSequenceLengthにかける()
        {
            foreach (var s in Data.RgiEmojiSequenceList)
            {
                var (type, length) = GraphemeBreak.GetEmojiSequence(s);
                Assert.Equal(s.Length, length);
                Assert.NotEqual(EmojiSequenceType.NotEmoji, type);
            }
        }

        /// <summary>
        /// <see cref="Data.RgiEmojiSequenceList"/> 前後に無関係の文字を挟んでみて、ちゃんと絵文字シーケンスの部分だけ抜き出されてるか見てみる。
        /// </summary>
        [Fact]
        public void Rgi絵文字シーケンスの前後にAsciiを挟んでからGetEmojiSequenceLengthにかける()
        {
            const string NonEmoji = "abc";
            foreach (var s in Data.RgiEmojiSequenceList)
            {
                var s2 = NonEmoji + s + NonEmoji;
                EmojiSequence emoji;

                var span = s2.AsSpan();

                // 非絵文字部分、常に0が返ってきて1文字進めればいいはず。
                for (int i = 0; i < NonEmoji.Length; i++)
                {
                    emoji = GraphemeBreak.GetEmojiSequence(span);
                    Assert.Equal(EmojiSequenceType.NotEmoji, emoji.Type);
                    span = span.Slice(emoji.LengthInUtf16);
                }

                // 絵文字部分、元の文字列と同じはず。
                emoji = GraphemeBreak.GetEmojiSequence(span);
                Assert.NotEqual(EmojiSequenceType.NotEmoji, emoji.Type);
                Assert.Equal(s.Length, emoji.LengthInUtf16);
                span = span.Slice(emoji.LengthInUtf16);

                // 非絵文字部分、常に0が返ってきて1文字進めればいいはず。
                for (int i = 0; i < NonEmoji.Length; i++)
                {
                    emoji = GraphemeBreak.GetEmojiSequence(span);
                    Assert.Equal(EmojiSequenceType.NotEmoji, emoji.Type);
                    span = span.Slice(emoji.LengthInUtf16);
                }

                // 最後まで読み切ったはず。
                Assert.Equal(0, span.Length);
            }
        }
    }
}
