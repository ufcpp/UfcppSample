using System;

class FieldInitialization
{
    public static void Run()
    {
        // 初期化せずにフィールドを読んでみる(規定値が入っている)
        var a = new DefaultValues();
        Console.WriteLine(a.i);
        Console.WriteLine(a.x);
        Console.WriteLine((int)a.c); // '\0' (ヌル文字)は表示できないので数値化して表示
        Console.WriteLine(a.b); // False
        Console.WriteLine(a.s == null); // null は表示できないので比較で。True になる
    }
}

class DefaultValues
{
    public int i;
    public double x;
    public char c;
    public bool b;
    public string s;
}
