using System;
using System.Collections.Generic;

namespace RgiSequenceFinder.TableGenerator
{
    struct GroupedEmojis
    {
        public List<(Keycap key, int index)> Keycaps { get; }
        public List<(RegionalIndicator code, int index)> RegionFlags { get; }
        public List<(TagSequence tag, int index)> TagFlags { get; }
        public int[] SkinTones { get; }

        /// <summary>
        /// いったん残すけど、<see cref="Singlulars"/> の考え方でうまくいったら消す。
        /// </summary>
        public List<(string emoji, int index)> Others { get; }

        /// <summary>
        /// RGI 絵文字シーケンスのうち半分以上は、
        /// - 1文字で絵文字判定を受ける
        /// - 1文字 + FE0F で絵文字判定を受ける
        /// のどちらかなので、char キーの辞書に置き換えたい(string アロケーション/参照/GetHashCode除けのため)。
        ///
        /// ただ、1文字といっても符号点的に1文字で、UTF-16 的には2文字になりえるものの、
        /// それも非 BMP (UTF-16 だとサロゲートペアになる)の文字の high surrogate は D83C, D83D, D83E の3つしかあり得ない。
        ///
        /// なので、
        /// - x = FE0F なしなら 0、ありなら 1
        /// - y = BMP 文字なら 0、D83C 開始文字なら 1, D83D 開始文字なら 2, D83E 開始文字なら 3
        /// (BMP 文字はそのまま、SMP 文字は low surrogate のみをキーにする)
        /// として、Singlulars[x, y].Add((c, index)) でリスト化する。
        /// </summary>
        public List<(char c, int index)>?[,] Singlulars { get; }

        /// <summary>
        /// <see cref="Singlulars"/> に収まらないものは大半が 1F000 台の文字なので。
        /// 同じく BMP or UTF-16 で D83C, D83D, D83E 開始の4つに分類してみる。
        /// BMP 文字なら 0、D83C 開始文字なら 1, D83D 開始文字なら 2, D83E 開始文字なら 3。
        /// </summary>
        public List<(string emoji, int index)>?[] Plurals { get; }

        public static GroupedEmojis Create() => new(Data.RgiEmojiSequenceList);

        public GroupedEmojis(string[] data)
        {
            var keycaps = Keycaps = new();
            var regionFlags = RegionFlags= new();
            var tagFlags = TagFlags = new();
            var skinTones = SkinTones = new int[5];
            var others = Others = new();
            var singulars = Singlulars = new List<(char, int)>[2, 4];
            var plurals = Plurals = new List<(string, int)>[4];

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
                        if (seq.Length == 1)
                        {
                            (singulars[0, 0] ??= new()).Add((seq[0], i));
                        }
                        else if (seq.Length == 2)
                        {
                            if (seq[1] == '\uFE0F') (singulars[1, 0] ??= new()).Add((seq[0], i));
                            else if (seq[0] == '\uD83C') (singulars[0, 1] ??= new()).Add((seq[1], i));
                            else if (seq[0] == '\uD83D') (singulars[0, 2] ??= new()).Add((seq[1], i));
                            else if (seq[0] == '\uD83E') (singulars[0, 3] ??= new()).Add((seq[1], i));
                        }
                        else if (seq.Length == 3 && seq[2] == '\uFE0F')
                        {
                            if (seq[0] == '\uD83C') (singulars[1, 1] ??= new()).Add((seq[1], i));
                            else if (seq[0] == '\uD83D') (singulars[1, 2] ??= new()).Add((seq[1], i));
                            else if (seq[0] == '\uD83E') (singulars[1, 3] ??= new()).Add((seq[1], i));
                        }
                        else
                        {
                            if (seq[0] == '\uD83C') (plurals[1] ??= new()).Add((seq, i));
                            else if (seq[0] == '\uD83D') (plurals[2] ??= new()).Add((seq, i));
                            else if (seq[0] == '\uD83E') (plurals[3] ??= new()).Add((seq, i));
                            else if(!char.IsSurrogate(seq[0])) (plurals[0] ??= new()).Add((seq, i));
                        }

                        others.Add((seq, i));
                        break;
                    case EmojiSequenceType.Keycap:
                        keycaps.Add((emoji.Keycap, i));
                        break;
                    case EmojiSequenceType.Flag:
                        regionFlags.Add((emoji.Region, i));
                        break;
                    case EmojiSequenceType.SkinTone:
                        skinTones[(int)emoji.SkinTone] = i;
                        break;
                    case EmojiSequenceType.Tag:
                        tagFlags.Add((emoji.Tags, i));
                        break;
                }
            }
        }
    }
}
