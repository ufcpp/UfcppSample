using System;

namespace ConsoleApp1.Stackalloc
{
    class Program
    {
        static void Main()
        {
            // C# 7.2 から、Span<T> を使えば safe なコンテキストで stackalloc を呼べるようになった。
            Span<int> x0 = stackalloc int[3];

            // 加えて、C# 7.3 で、stackalloc に初期化子を付けれるようになった。
            // 配列初期化子でできることは stackalloc でもできるように。

            // 単に初期化子を追加。
            Span<int> x1 = stackalloc int[3] { 0xEF, 0xBB, 0xBF };

            // 初期化子があるからサイズは省略。
            Span<int> x2 = stackalloc int[] { 0xEF, 0xBB, 0xBF };

            // 初期化子から推論できるときは型名も省略可能。
            Span<int> x3 = stackalloc[] { 0xEF, 0xBB, 0xBF };

            Console.WriteLine(x3.Length); // ちゃんと 3 が取れる

            for (int i = 0; i < x3.Length; i++)
            {
                // 書き方が違うだけで x1, x2, x3 の内容は全部一緒。
                Console.WriteLine((x1[i], x2[i], x3[i]));
            }
        }
    }
}
