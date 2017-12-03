using System;

class OverloadSample
{
    public static void Run()
    {
        WriteTypeAndValue("サンプル"); // WriteTypeAndValue(string) が呼ばれる
        WriteTypeAndValue(13);         // WriteTypeAndValue(int)    が呼ばれる
        WriteTypeAndValue(3.14159265); // WriteTypeAndValue(double) が呼ばれる
    }

    /// <summary>
    /// 型名と値を出力する(string 版)。
    /// </summary>
    static void WriteTypeAndValue(string s)
    {
        Console.Write("文字列 : {0}\n", s);
    }

    /// <summary>
    /// 型名と値を出力する(int 版)。
    /// </summary>
    static void WriteTypeAndValue(int n)
    {
        Console.Write("整数   : {0}\n", n);
    }

    /// <summary>
    /// 型名と値を出力する(double 版)。
    /// </summary>
    static void WriteTypeAndValue(double x)
    {
        Console.Write("実数   : {0}\n", x);
    }

    // F は、引数の型が違うので大丈夫
    static void F(int x) { }
    static void F(string x) { }

#if InvalidCode
    // G は、引数の型まで一緒で、名前だけ違う。これはコンパイル エラー
    static void G(int x) { }
    static void G(int y) { }

    // H は、引数が一致していて、戻り値だけ違う。これもコンパイル エラー
    static int H() => 1;
    static string H() => "";
#endif
}
