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
}
