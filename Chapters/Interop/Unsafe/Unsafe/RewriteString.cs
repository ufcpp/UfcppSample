namespace Unsafe.RewriteString
{
using System;

class Program
{
    static void Main()
    {
        // C# の string は書き換えできないはず
        var s1 = "-----";

        // 参照型なので、同じインスタンスを見てる
        // 書き換えれないからこそ、インスタンスの共有が安全
        var s2 = s1;

        // 実際には、C# の string は書き換えれる
        unsafe
        {
            fixed (char* c = s1)
            {
                c[2] = 'X';
            }
        }

        Console.WriteLine(s1); // --X--
        Console.WriteLine(s2); // 同じものを見てるので、こちらにも書き換えの影響が出てて --X--
    }
}
}
