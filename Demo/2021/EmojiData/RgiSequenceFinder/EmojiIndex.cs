using System;
using System.Text;
using System.Text.Unicode;

namespace RgiSequenceFinder
{
    /// <summary>
    /// RGI 絵文字シーケンスを弁別するための値。
    /// emoji-data.json に並んでる順をそのままインデックスとして利用(0開始)。
    ///
    /// 「ただ、RGI 絵文字シーケンスじゃなかったときに元の文字の符号点を素通し」みたいな仕様にしたくて、
    /// インデックスなのか符号点なのかを弁別するためのフラグを持ってる。
    /// </summary>
    /// <remarks>
    /// 基本、「RGI 絵文字シーケンスじゃなければ何も書き込まない」でやるので単独だと「符号点素通し」処理は起きないものの…
    ///
    /// 以下の2つの状況で「符号点素通し」が必要に。
    /// - RGI 内にない ZWJ sequence で、絵文字 + 非絵文字 みたいな結果になることがある
    /// - RGI に入ってない Flag sequence を "AA" みたいな ASCII 文字列化してみようかと思ってる
    ///
    /// 実際にはフラグを持っているというか、
    /// - 絵文字だったらその絵文字画像インデックスを入れる
    /// - 非絵文字だったら符号点を ^ でビット反転して入れる
    /// - 最上位ビットが 1 かどうかでインデックスか符号点かを弁別
    /// とかでやってる。
    /// </remarks>
    public readonly struct EmojiIndex : IEquatable<EmojiIndex>
    {
        private readonly int _value;

        public EmojiIndex(int index) => _value = index;

        public static implicit operator EmojiIndex(int index) => new(index);

        public EmojiIndex(char c) => _value = ~c;

        public EmojiIndex(char high, char low) => _value = ~(new Rune(high, low).Value);

        public int Index =>
#if DEBUG
            _value >= 0 ? _value : throw new IndexOutOfRangeException();
#else
            _value;
#endif

        public Rune Rune =>
#if DEBUG
            _value < 0 ? new Rune(~_value) : throw new IndexOutOfRangeException();
#else
            new Rune(~_value);
#endif

        public bool IsIndex => _value >= 0;

        public int WriteUtf16(Span<char> buffer)
        {
            if (_value >= 0) return 0;

            // 内部的に UTF-16 なものを1度符号点にしたのを割とすぐ再度 UTF-16 エンコードする作りなのがちょっともったいない感じはするものの。
            // そんなに高頻度でここには来ないと思うし、サロゲートペア2個を int にパッキングすると Index との区別が面倒になりそうだしやめとく。
            var r = new Rune(~_value);
            return r.EncodeToUtf16(buffer);
        }

        public bool Equals(EmojiIndex other) => _value == other._value;
        public override bool Equals(object? obj) => obj is EmojiIndex other && Equals(other);
        public override int GetHashCode() => _value.GetHashCode();
        public static bool operator ==(EmojiIndex x, EmojiIndex y) => x.Equals(y);
        public static bool operator !=(EmojiIndex x, EmojiIndex y) => !x.Equals(y);
    }
}
