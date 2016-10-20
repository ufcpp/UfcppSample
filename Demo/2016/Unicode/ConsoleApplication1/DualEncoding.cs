using System;
using System.Text;

namespace ConsoleApplication1
{
    class DualEncoding
    {
        public static void Run()
        {
            var s = "Latin-1, abc àèì";
            byte[] latin1 = Encoding.GetEncoding(1252).GetBytes(s); // Latin-1
            byte[] utf16 = Encoding.Unicode.GetBytes(s);           // UTF-16

            for (int i = 0; i < latin1.Length; i++)
            {
                var x = latin1[i];
                var y = utf16[2 * i];

                // Latin-1 と UTF-16 の差は、バイト数の差だけ
                // UTF-16 の方を1バイト飛ばしで読めば、必ず同じ値が入っている
                if (x != y) throw new InvalidOperationException(); // ここは絶対に通らない
            }
        }
    }
}
