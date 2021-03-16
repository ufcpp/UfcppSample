using System;
using System.Linq;
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
            // 単独で RGI になってるものをあえて選んでるはずなのを一応確認。
            Span<int> indexes = stackalloc int[1];
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
    }
}
