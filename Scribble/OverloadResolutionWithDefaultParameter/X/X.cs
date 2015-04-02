using System;

public class X
{
    public static void HogeMethod(bool isFuga = false) { Console.WriteLine("a"); }
    public static void HogeMethod(params string[] fugas) { Console.WriteLine("b"); }
}
