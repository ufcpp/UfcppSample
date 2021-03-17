using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Xunit;

namespace RgiSequenceFinder.Test
{
    /// <summary>
    /// 過剰に FE0F がついてるとか、過剰に ZWJ でつながってるとか、RGI にないパターンで肌色選択が掛かってるのとかを分割したり余計な分を削ったりのテスト。
    /// </summary>
    public class FallbackFindIndexTest
    {
        [Theory]
        [InlineData("✨")] // 2728, Sparkles
        [InlineData("❌")] // 274C, Cross Mark
        [InlineData("⬛")] // 2B1B, Black Large Square
        [InlineData("🐈")] // 1F408, Cat
        public void 末尾FE0Fを削る(string emoji)
        {
            Span<int> indexes = stackalloc int[1];

            // 単独で RGI になってるものをあえて選んでるはずなのを一応確認。
            var (len, indexWritten) = RgiTable.Find(emoji, indexes);

            Assert.Equal(1, indexWritten);
            Assert.Equal(emoji.Length, len);

            // 元データと照会。
            var indexFromData = Data.RgiEmojiSequenceList.TakeWhile(x => x != emoji).Count();
            Assert.Equal(indexFromData, indexes[0]);

            // FE0F 足してみる。
            var fe0fAdded = emoji + "\uFE0F";
            (len, indexWritten) = RgiTable.Find(fe0fAdded, indexes);

            Assert.Equal(1, indexWritten);
            Assert.Equal(fe0fAdded.Length, len);
            Assert.Equal(indexFromData, indexes[0]);
        }

        [Fact]
        public void 未サポート旗()
        {
            Span<int> indexes = stackalloc int[1];

            // 未サポート旗、黒旗だけの絵文字に fallback するように作った。
            var (len, indexWritten) = RgiTable.Find("\U0001F3F4", indexes);
            var fallbackIndex = indexes[0];

            // gbsct 🏴󠁧󠁢󠁳󠁣󠁴󠁿
            // iOS とかではちゃんとスコットランド旗が出る。
            // Windows でも Twitter とかにコピペするとスコットランド旗画像に指し変わる。
            var supported = "\U0001F3F4\U000E0067\U000E0062\U000E0073\U000E0063\U000E0074\U000E007F";

            // この場合は普通に対応するインデックスが取れる。
            (len, indexWritten) = RgiTable.Find(supported, indexes);
            Assert.Equal(1, indexWritten);
            Assert.Equal(supported.Length, len);
            Assert.NotEqual(fallbackIndex, indexes[0]);

            // jp13 (東京都) 🏴󠁪󠁰󠀱󠀳󠁿
            // Emoji tag sequence の仕様上、1F3F4 (黒旗)の後ろに ISO 3166-2 (行政区画コード)に相当するタグ文字が並んでるとその区の旗という扱いになる。
            // 原理的にいくらでもサポートできる旗を増やせるというだけで RGI に入ってるのは gbeng, gbsct, gbwls だけ。
            // (でも、Emojipedia には並んでたりする。 https://emojipedia.org/flag-for-tokyo-jp13/)
            var unsupported = "\U0001F3F4\U000E006A\U000E0070\U000E0031\U000E0033\U000E007F";

            (len, indexWritten) = RgiTable.Find(unsupported, indexes);
            Assert.Equal(1, indexWritten);
            Assert.Equal(unsupported.Length, len);
            Assert.Equal(fallbackIndex, indexes[0]);
        }

        [Fact]
        public void 未サポート肌色修飾()
        {
            // 肌色が関係ない基本絵文字、動物とかを適当に選んだもの。
            var emojis = new[]
            {
                "♈", "♉", "♊", "♋", "♌", "♍", "♎", "♏", "♐", "♑", "♒", "♓",
                "🐭", "🐮", "🐯", "🐰", "🐲", "🐍", "🐴", "🐏", "🐵", "🐔", "🐶", "🐗",
            };

            var skinTones = new[] { "🏻", "🏼", "🏽", "🏾", "🏿", };

            // 基本絵文字と skin tone を交互に並べる。
            string concat()
            {
                var sb = new StringBuilder();

                for (int i = 0; i < emojis.Length; i++)
                {
                    sb.Append(emojis[i]);
                    sb.Append(skinTones[i % skinTones.Length]);
                }

                return sb.ToString();
            }

            static HashSet<int> toIndex(string[] strings)
            {
                Span<int> indexes = stackalloc int[1];
                var set = new HashSet<int>();
                foreach (var st in strings)
                {
                    RgiTable.Find(st, indexes);
                    set.Add(indexes[0]);
                }
                return set;
            }

            var s = concat().AsSpan();
            var skinToneIndexes = toIndex(skinTones);
            Span<int> indexes = stackalloc int[2];

            while (true)
            {
                // RGI に含まれていないので、基本絵文字と skin tone の2文字分返ってくる。
                var (len, indexWritten) = RgiTable.Find(s, indexes);
                Assert.Equal(2, indexWritten);
                Assert.Contains(indexes[1], skinToneIndexes);

                s = s.Slice(len);
                if (s.Length == 0) break;
            }
        }
    }
}
