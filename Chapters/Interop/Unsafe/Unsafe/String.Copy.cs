namespace Unsafe.StringCopy
{
using System;

class Program
{
    static void Main()
    {
        // C# の string は書き換えできないはず
        // 書き換えれないもののコピー作って意味あるの？という疑問。
        var s = "-----";

        // s と同じものを参照
        var s1 = s;

        // コピーして新しいのを作ったので別の場所を参照
        var s2 = string.Copy(s);

        // どっちも同じ見た目だし
        Console.WriteLine(s1); // -----
        Console.WriteLine(s2); // -----

        // C# の == は中身の比較だし
        Console.WriteLine(s1 == s2); // true

        // ハッシュ値も一致するし
        Console.WriteLine(s1.GetHashCode() == s2.GetHashCode()); // true

        // しいて言うなら、ReferenceEquals は不一致
        Console.WriteLine(ReferenceEquals(s, s1)); // true
        Console.WriteLine(ReferenceEquals(s, s2)); // false

        // 実際には、C# の string は書き換えれる
        unsafe
        {
            fixed (char* c = s1)
            {
                c[2] = 'X';
            }
        }

        Console.WriteLine(s1); // --X--: s を書き換えてしまったので、同じ場所を見てる s1 も変わる
        Console.WriteLine(s2); // -----: こっちは別コピーなので元のまま
    }
}
}
