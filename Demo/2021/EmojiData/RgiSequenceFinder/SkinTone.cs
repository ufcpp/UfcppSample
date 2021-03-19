namespace RgiSequenceFinder
{
    /// <summary>
    /// 絵文字の肌色。
    /// </summary>
    /// <remarks>
    /// 絵文字の闇その3。
    ///
    /// 医療法面で紫外線耐性の分類で Fitzpatrick skin typing ってのがあるらしく、それを元に肌色を付けることにしたらしい。
    /// 絵文字的には typ1-2, 3, 4, 5, 6 (Fitzpatrick type の1-2を一緒くた)の5種。
    ///
    /// 1F3FB-1F3FF の連番。
    /// (UTF-16 だと high surrogate が D83C 固定、low surrogate が DFFB-DFFF の連番。)
    /// テーブル的にもそのままこの順で0開始の連番で並べて置く想定。
    ///
    /// 本来、skin tone は単体の絵文字扱いしないんだけど…
    /// 絵文字 + skin tone の組み合わせに対応していないとき、skin tone 部分をその色に対応する四角を表示するみたいな仕様があって、
    /// 肌色四角の絵を独立して用意することがほとんど。
    /// この実装でも肌色四角を出す想定。
    ///
    /// 後々追加された髪型選択 1F9B0～1F9B3 は単独表示しなくてもよさそうなんで、Unicode 仕様上も skin tone だけ浮いてたりする。
    /// </remarks>
    public enum SkinTone : sbyte
    {
        Type2,
        Type3,
        Type4,
        Type5,
        Type6,

        /// <summary>
        /// 構造体にぎちぎちにパッキングしたいことがあるので <see cref="System.Nullable{T}"/> を避けて、「skin tone が見つからなかった時は負」みたいな運用する。
        /// </summary>
        None = -1,
    }

    /// <summary>
    /// <see cref="SkinTone"/> 0～2個詰め込んだ構造体。
    /// </summary>
    /// <remarks>
    /// <see cref="SkinTone"/> の使い方、RGI テーブルを引くときには、
    /// - まず skin tone を抽出
    /// - skin tone を削った文字列でテーブル引き
    /// - skin tone から計算できるオフセットを足す
    /// - RGI ZWJ sequence 中にある skin tone は1個か2個
    /// みたいな前提があるし、他のデータと一緒に1つの構造体にパッキングするんで1バイトに2 <see cref="SkinTone"/> を詰め込むことに。
    ///
    /// 長さ(0, 1, 2) 2ビット、2個目の tone 3ビット、1個目の tone 3ビットでパッキング。
    /// </remarks>
    public readonly struct SkinTonePair
    {
        public readonly byte Value;

        public SkinTonePair(byte value) => Value = value;

        public SkinTonePair(SkinTone tone1, SkinTone tone2)
        {
            if (tone1 < 0)
            {
                Value = 0;
            }
            else if (tone2 < 0)
            {
                Value = (byte)(
                    0b0100_0000
                    | (byte)tone1);
            }
            else
            {
                Value = (byte)(
                    0b1000_0000
                    | (byte)tone1
                    | ((byte)tone2 << 3));
            }
        }

        /// <summary>
        /// 長さ。0～2。
        /// </summary>
        /// <remarks>
        /// この構造体が default のときに 0 になるようにしてある。
        /// </remarks>
        public int Length => Value >> 6;

        /// <summary>
        /// <see cref="SkinTone"/> 1個目。
        /// カップル絵文字とかで前の人の肌色。
        /// ZWJ sequence 的に、2符号点目に出てくる。
        /// </summary>
        public SkinTone Tone1 => (SkinTone)(Value & 0b111);

        /// <summary>
        /// <see cref="SkinTone"/> 2個目。
        /// カップル絵文字とかで後の人の肌色。
        /// ZWJ sequence 的に、最後の符号点に出てくる。
        /// </summary>
        public SkinTone Tone2 => (SkinTone)((Value >> 3) & 0b111);
    }
}
