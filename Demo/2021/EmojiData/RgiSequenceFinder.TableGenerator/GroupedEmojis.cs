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
        /// https://www.unicode.org/Public/emoji/13.1/emoji-sequences.txt で Basic_Emoji になってるやつはこのパターンになるはず。
        /// (なので「1文字の」(singular)とか言わず、Basics とか BasicEmojis でもいいかも。)
        ///
        /// RGI 絵文字シーケンスのうち半分くらいは、
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
        /// RGI_Emoji_Modifier_Sequence と RGI_Emoji_ZWJ_Sequence だけが残ってるはず。
        /// </summary>
        /// <remarks>
        /// skin tone によるバリエーションは <see cref="Data.UnvariedRgiEmojiSequenceList"/> を作る側で処理済み。
        /// </remarks>
        public List<(string emoji, int index, byte skinVariationType)> Others { get; }

        // Role 系、Gendered 系、Hair 系の ZWJ sequence もパターン決まってるので分けた方がいいかも。
        //
        // Roke 系     = (1F468 | 1F469 | 1F9D1) (1F3FB-1F3FF)+ 200D (職業)
        // Gendered 系 = (人系絵文字) (1F3FB-1F3FF | FE0F)+ 200D (2640 | 2642) FE0F
        // Hair 系     = (1F468 | 1F469 | 1F9D1) (1F3FB-1F3FF)+ 200D (1F9B0-1F9B3)
        //
        // 家族絵文字みたいに skin tone が複数紛れてるものでなければ、emoji-data.json 上のインデックスは肌色で連番だと思ってよさげ。
        // (emoji-data.json の skin_variations の並びは 1F3FB-1F3FF 順で歯抜けもないかも。)
        //
        // Gendered 系で2文字目に FE0F が付いてるのは 26F9 FE0F (person bounsing ball)だけっぽい。
        //
        // 2人家族系の絵文字も両端が (1F468 | 1F469 | 1F9D1) (1F3FB-1F3FF)+
        // (これも、emoji-data.json 内、両端の 1F3FB-1F3FF に合わせて一定順序で skin_variations 並んでるかも。)
        //
        // 1F468 👨 man
        // 1F469 👩 woman
        // 1F9D1 🧑 person (gender neutral)
        //
        // 2640 ♀ female sign
        // 2642 ♂ male sign

        public static GroupedEmojis Create() => new(Data.UnvariedRgiEmojiSequenceList);

        public GroupedEmojis((string emoji, ushort index, byte variationType)[] data)
        {
            var keycaps = Keycaps = new();
            var regionFlags = RegionFlags= new();
            var tagFlags = TagFlags = new();
            var skinTones = SkinTones = new int[5];
            var others = Others = new();
            var singulars = Singlulars = new List<(char, int)>[2, 4];

            for (int i = 0; i < data.Length; i++)
            {
                var seq = data[i].emoji;

                var emoji = GraphemeBreak.GetEmojiSequence(seq);
                var (type, len) = emoji;

                if (len != seq.Length) throw new InvalidOperationException("ないはず");

                switch (type)
                {
                    default:
                    case EmojiSequenceType.NotEmoji:
                        throw new InvalidOperationException("ないはず");
                    case EmojiSequenceType.Other:
                        List<(char, int)>? singular = null;
                        char c = '\0';

                        if (seq.Length == 1)
                        {
                            singular = (singulars[0, 0] ??= new());
                            c = seq[0];
                        }
                        else if (seq.Length == 2)
                        {
                            if (seq[1] == '\uFE0F')
                            {
                                singular = (singulars[1, 0] ??= new());
                                c = seq[0];
                            }
                            else
                            {
                                if (seq[0] == '\uD83C') singular = (singulars[0, 1] ??= new());
                                else if (seq[0] == '\uD83D') singular = (singulars[0, 2] ??= new());
                                else if (seq[0] == '\uD83E') singular = (singulars[0, 3] ??= new());
                                c = seq[1];
                            }
                        }
                        else if (seq.Length == 3 && seq[2] == '\uFE0F')
                        {
                            if (seq[0] == '\uD83C') singular = (singulars[1, 1] ??= new());
                            else if (seq[0] == '\uD83D') singular = (singulars[1, 2] ??= new());
                            else if (seq[0] == '\uD83E') singular = (singulars[1, 3] ??= new());
                            c = seq[1];
                        }

                        if (singular is not null) singular.Add((c, data[i].index));

                        // singular テーブルに含めた場合でも、variationType を持ってる文字は others に入れる。
                        if (singular is null || data[i].variationType > 0) others.Add((seq, data[i].index, data[i].variationType));
                        break;
                    case EmojiSequenceType.Keycap:
                        keycaps.Add((emoji.Keycap, data[i].index));
                        break;
                    case EmojiSequenceType.Flag:
                        regionFlags.Add((emoji.Region, data[i].index));
                        break;
                    case EmojiSequenceType.SkinTone:
                        skinTones[(int)emoji.SkinTone] = data[i].index;
                        break;
                    case EmojiSequenceType.Tag:
                        tagFlags.Add((emoji.Tags, data[i].index));
                        break;
                }
            }
        }
    }
}
