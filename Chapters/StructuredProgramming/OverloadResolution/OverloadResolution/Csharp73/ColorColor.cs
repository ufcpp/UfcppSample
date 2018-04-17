using System;

struct Color
{
    public byte R;
    public byte G;
    public byte B;

    // どちらも M() で呼べるメソッド。
    public void M(int x = 0) => Console.WriteLine("Instance");
    public static void M(Color c = default) => Console.WriteLine("static");

    // 参考までに、オーバーロードがない場合。
    public void Instance() { }
    public static void Static() { }
}

class Program
{
    // C# では、型名とプロパティ名が同じプロパティを作れる。
    static Color Color { get; set; }

    static void Main()
    {
        // これは「プロパティのColor」(C# 7.2以前でも行ける)。
        Color.Instance();

        // これが「型のColor」(C# 7.2以前でも行ける)。
        Color.Static();

        // これだと、この Color が型名かプロパティかが区別できない。
        // C# 7.3 でも不明瞭エラー。
#if Uncompilable
        Color.M();
#endif

        // C# 7.3 なら、以下の書き方で呼び分け可能(これまでは不明瞭エラー)。
        // 「プロパティのColor」。
        Program.Color.M();
        // 「型のColor」。
        global::Color.M();
    }
}
