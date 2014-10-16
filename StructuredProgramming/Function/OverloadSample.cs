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
}
