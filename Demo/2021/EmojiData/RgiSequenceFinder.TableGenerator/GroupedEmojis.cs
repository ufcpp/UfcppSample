using System;
using System.Collections.Generic;

namespace RgiSequenceFinder.TableGenerator
{
    struct GroupedEmojis
    {
        public List<(Keycap key, int index)> Keycaps { get; }
        public List<(RegionalIndicator code, int index)> RegionFlags { get; }
        public List<(TagSequence tag, int index)> TagFlags { get; }
        public List<(string emoji, int index)> Others { get; }

        public static GroupedEmojis Create() => new(Data.RgiEmojiSequenceList);

        public GroupedEmojis(string[] data)
        {
            var keycaps = new List<(Keycap key, int index)>();
            var regionFlags = new List<(RegionalIndicator code, int index)>();
            var tagFlags = new List<(TagSequence tag, int index)>();
            var others = new List<(string emoji, int index)>();

            for (int i = 0; i < data.Length; i++)
            {
                var seq = data[i];

                var emoji = GraphemeBreak.GetEmojiSequence(seq);
                var (type, len) = emoji;

                if (len != seq.Length) throw new InvalidOperationException("ないはず");

                switch (type)
                {
                    default:
                    case EmojiSequenceType.NotEmoji:
                        throw new InvalidOperationException("ないはず");
                    case EmojiSequenceType.Other:
                        others.Add((seq, i));
                        break;
                    case EmojiSequenceType.Keycap:
                        keycaps.Add((emoji.Keycap, i));
                        break;
                    case EmojiSequenceType.Flag:
                        regionFlags.Add((emoji.Region, i));
                        break;
                    case EmojiSequenceType.Tag:
                        tagFlags.Add((emoji.Tags, i));
                        break;
                }
            }

            Keycaps = keycaps;
            RegionFlags = regionFlags;
            TagFlags = tagFlags;
            Others = others;
        }
    }
}
