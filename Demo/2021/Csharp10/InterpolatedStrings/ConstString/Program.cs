// {} 内に文字列以外の const を指定できない理由の1つが、ToString のカルチャー依存:

using System.Globalization;

// 東南アジアの多くの国は . を小数点に使う。
Thread.CurrentThread.CurrentCulture = new CultureInfo("ja-jp");
Console.WriteLine(1.234);

// 大陸ヨーロッパの多くの国は , を小数点に使う。
Thread.CurrentThread.CurrentCulture = new CultureInfo("fr-fr");
Console.WriteLine(1.234);

class Valid
{
    const string A = "Abc";
    const string B = "Xyz";
    public const string C = $"{nameof(A)}: {A}, {nameof(B)}: {B}"; // "A: Abc, B: Xyz"
}

#if Error
class Invalid
{
    const int A = 1;
    const string C = $"{A}"; // A が文字列じゃないので $"" の結果を const にできない。}
}
#endif
