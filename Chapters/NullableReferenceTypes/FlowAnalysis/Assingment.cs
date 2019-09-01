using System;

namespace FlowAnalysis.Assingment
{
    class Program
    {
        static void Main()
        {
            // null 許容で宣言されていても、
            string? s;

            // ちゃんと有効な値を代入すれば
            s = "abc";

            // 警告は出なくなる。
            Console.WriteLine(s.Length);

            // 逆に null を代入すると、
            s = null;

            // それ以降警告が出る。
            Console.WriteLine(s.Length);
        }
    }
}
