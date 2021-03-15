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

            // Regional Indicator ���g��������������ȏ㑝����Ƃ͎v���Ȃ����ǁA�ꉞ�o�[�W�����ɂ���ĕς��\���͂���̂Œ��ӁB
            // ���Ȃ��Ƃ� Emoji 2.0 (Unicode 6.0) �` Unicode 13.1 �ł�258�����̂͂��B
            Assert.Equal(258, count);
        }

        [Fact]
        public void TestTagSequence()
        {
            // ���s�� RGI ���� gbeng, gbsct, gbwls ��3�����̂͂��B
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
        /// <see cref="Data.RgiEmojiSequenceList"/> �ɓ��ꂽ�����͑S���u�Ō�܂�1�Ȃ��̊G�����V�[�P���X�v������󂯂�͂��B
        /// </summary>
        [Fact]
        public void Rgi�G�����V�[�P���X�S�̂�GetEmojiSequenceLength�ɂ�����()
        {
            foreach (var s in Data.RgiEmojiSequenceList)
            {
                var (type, length) = GraphemeBreak.GetEmojiSequence(s);
                Assert.Equal(s.Length, length);
                Assert.NotEqual(EmojiSequenceType.NotEmoji, type);
            }
        }

        /// <summary>
        /// <see cref="Data.RgiEmojiSequenceList"/> �O��ɖ��֌W�̕���������ł݂āA�����ƊG�����V�[�P���X�̕������������o����Ă邩���Ă݂�B
        /// </summary>
        [Fact]
        public void Rgi�G�����V�[�P���X�̑O���Ascii������ł���GetEmojiSequenceLength�ɂ�����()
        {
            const string NonEmoji = "abc";
            foreach (var s in Data.RgiEmojiSequenceList)
            {
                var s2 = NonEmoji + s + NonEmoji;
                EmojiSequence emoji;

                var span = s2.AsSpan();

                // ��G���������A���0���Ԃ��Ă���1�����i�߂�΂����͂��B
                for (int i = 0; i < NonEmoji.Length; i++)
                {
                    emoji = GraphemeBreak.GetEmojiSequence(span);
                    Assert.Equal(EmojiSequenceType.NotEmoji, emoji.Type);
                    span = span.Slice(emoji.LengthInUtf16);
                }

                // �G���������A���̕�����Ɠ����͂��B
                emoji = GraphemeBreak.GetEmojiSequence(span);
                Assert.NotEqual(EmojiSequenceType.NotEmoji, emoji.Type);
                Assert.Equal(s.Length, emoji.LengthInUtf16);
                span = span.Slice(emoji.LengthInUtf16);

                // ��G���������A���0���Ԃ��Ă���1�����i�߂�΂����͂��B
                for (int i = 0; i < NonEmoji.Length; i++)
                {
                    emoji = GraphemeBreak.GetEmojiSequence(span);
                    Assert.Equal(EmojiSequenceType.NotEmoji, emoji.Type);
                    span = span.Slice(emoji.LengthInUtf16);
                }

                // �Ō�܂œǂݐ؂����͂��B
                Assert.Equal(0, span.Length);
            }
        }
    }
}
