using System;

public class Program
{
    static void Main()
    {
#nullable enable
        E1(null); // 警告が出る

#nullable disable
        E1(null); // 警告が出ない
    }

#nullable enable
    // 有効化したのでここでは string で非 null、string? で null 許容。
    static int E1(string s) => s.Length;
    static int? E2(string? s) => s?.Length;

#nullable disable
    // 無効化したので string に null が入っている可能性あり。
    // string? とは書けない(書くだけで警告になる)。
    static int D1(string s) => s.Length;

#nullable restore
    // 1つ前のコンテキストに戻す。
    // この場合、disable から enable に戻る。
    static int? R1(string? s) => s?.Length;
}
