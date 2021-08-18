using InvariantGlobalization;
using System.Globalization;

// CurrentCulture をあえてフランスに。
// 大陸ヨーロッパは小数点が , な事が多く。
Thread.CurrentThread.CurrentCulture = new CultureInfo("fr-fr");

// 12,345 になるわ、
// dd/MM/yyyy になるわ。
Console.WriteLine($@"{12.345}
{DateTimeOffset.Now}
{new DateOnly(2000, 1, 2)}
{new TimeOnly(11, 5, 30)}
");

// というか、CurrentCulture 取得はそれなりにコスト(ThreadStatic アクセスあり)かかるし、
// そもそも実行環境によって Format/Parse 結果が違うのが怖い。
// なので InvariantCulture にすると…
// 北米フォーマットになる。
// (IT 分野あるあるの「デフォルトは北米」。)
//
// MM/dd/yyyy とかちょっと…
// (英語圏でも Why do Americans? とか言われてるし、他のアメリカ大陸国からは It's only USA って言われてる。
// 元々英語の語順は変だし、イギリスも古くは MM/dd/yyyy だったけど周辺国の影響で更生されてそう。
// 元植民地が宗主国よりも変化に対して保守的になるのは結構あるあるらしい。)
Console.WriteLine(string.Create(CultureInfo.InvariantCulture, $@"{12.345}
{DateTimeOffset.Now}
{new DateOnly(2000, 1, 2)}
{new TimeOnly(11, 5, 30)}
"));

// まあとりあえず、常時 InvariantCulture を指定するオーバーロードを作る。
Console.WriteLine(Invariant.Format($@"{12.345}
{DateTimeOffset.Now}
{new DateOnly(2000, 1, 2)}
{new TimeOnly(11, 5, 30)}
"));

// で、さらに、日付フォーマットのデフォルトを O (ISO 8601 準拠)に変更。
// yyyy-MM-ddThh:mm:ss.fffffffK 相当。
Console.WriteLine(Iso8601.Format($@"{12.345}
{DateTimeOffset.Now}
{new DateOnly(2000, 1, 2)}
{new TimeOnly(11, 5, 30)}
"));
