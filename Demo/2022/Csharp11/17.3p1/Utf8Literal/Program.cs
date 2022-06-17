#pragma warning disable CS0183, CS8321

//## UTF-8 リテラル

// 本音を言うなら Utf8String とか「UTF-8 を表す型」が欲しいし、
// なんだったら string の中身を UTF-8 にしてほしいけども。
// それは課題が多くて、やるとしても長期計画とかになっちゃって厳しい。
//
// そこまでやらなくても今ある需要の幾分かを解決できる手段として「リテラルだけ」を提供。
// byte[] や Span<byte>、ReadOnlySpan<byte> に対して代入できる「UTF-8 バイト列を表すリテラル」が導入されることに。

//## u8 接尾辞
// ""u8 みたいに、"" の後ろに u8 接尾辞を付ける。

var utf8 = "aあ🐈"u8;

// ちなみに、var を使うと現状(17.3p1)は ReadOnlySpan<byte> 型になる。
// (もしかしたらリリースまでに ReadOnlySpan<byte> に変わるかも。)
Console.WriteLine(utf8 is ReadOnlySpan<byte>); // 常に true。

// aあ🐈 を UTF-8 化したものなので、
// 61 E3 81 82 F0 9F 90 88
// が表示される。
write(utf8);

static void write(ReadOnlySpan<byte> utf8)
{
    foreach (byte x in utf8) Console.Write($"{x:X2} ");
    Console.WriteLine();
}

//## アロケーション回避
// (u8 リテラルの仕様じゃなくて元からある仕様なものの)
// ReadOnlySpan<byte> に対して定数 byte[] を代入すると、アロケーションがなくなる最適化が掛かる。

ReadOnlySpan<byte> span = "aあ🐈"u8; // new byte[] のアロケーションは消える。
write(span);

// 元からそうなので、
// ReadOnlySpan<byte> span = new byte[] { 0x61, 0xE3, 0x81, 0x82, 0xF0, 0x9F, 0x90, 0x88 };
// でも全く同じ結果。

//## UTF-8 raw string literals
// raw string との組み合わせも可能。

var raw = """
    abc
    あいう
    """u8;

// a  b  c  \r \n あ       い       う
// 61 62 63 0D 0A E3 81 82 E3 81 84 E3 81 86
// (改行文字はソースコードの文字コード設定次第。git の auto-crlf には注意。)
write(raw);

//## const じゃない
// u8 リテラルは const にはなれない。
// new byte[] が const になれないので、それと一緒。

#if false
// これは「そもそも const byte[] とは書けない」ということでエラー。
const byte[] constUtf8 = "abc"u8;

// 既定値にも使えない。
void m(byte[] utf8 = "abc"u8) { }

// パターンにも使えない。
if (utf8 is "abc"u8) { }

// 属性の引数にも使えない。
[MyAttribute("abc"u8)]
void m() { }
#endif

// new byte[] なら属性の引数に使えるのに…
[MyAttribute(new byte[] { 0x61, 0xE3, 0x81 })] // これは OK。
void m() { }

// 属性の引数には const もしくは配列作成式(array creation expression)を渡せるんだけど、
// ""u8 は new byte[] として解釈されるにも関わらず、array creation じゃない。
// (これは C# 11 リリースまでにはもしかしたら改善されるかも。)

//## 不正な UTF-8
#if false
// サロゲートペアの片割れとか、C# 文字列リテラルとしては有効だけど、UTF-8 としては無効な文字列がある。
// そういう文字列リテラルに u8 を付けるとコンパイル エラーになる。
ReadOnlySpan<byte> ilformed = "\uD83D"u8; // CS9026 エラー。
#endif

//## 変更予定
#if false
// 17.3p1 時点では書けてしまうコード:
// 暗黙の変換はオーバーロード解決とか壊しちゃうみたいでやっぱやめるって。
// 今後、u8 語尾必須になる予定。
byte[] implicitUtf8 = "aあ🐈";

// 17.3p1 時点で書けないコード:
// これはできるようにするかも。
var concat = "abc"u8 + "def"u8;

// 今は↓みたいに書けるけど、上記の通り、暗黙の変換自体なくなる。
byte[] concat = "abc" + "def";

// 17.3p1 時点で書けないコード:
// 文字列補間もダメ。
// これは要望は出てるけども多分 C# 11 リリース時点ではやれなさそう。
byte[] interpolation = $"{utf8} + abc"u8;
#endif

class MyAttribute : Attribute
{
    public MyAttribute(byte[] bytes) { }
}
