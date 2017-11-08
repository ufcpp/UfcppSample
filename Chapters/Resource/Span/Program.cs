using System;

namespace Span
{
    class Program
    {
        static void Main(string[] args)
        {
            MinimalSample();
        }

        private static void MinimalSample()
        {
            // 長さ 8 で配列作成
            // C# の仕様で、全要素 0 で作られる
            var array = new int[8];

            // 配列の、2番目(0 始まりなので3要素目)から、3要素分の範囲
#if true
            var span = new Span<int>(array, 2, 3);
#else
            var span = array.AsSpan().Slice(2, 3);
#endif

            // その範囲だけを 1 に上書き
            for (int i = 0; i < span.Length; i++)
            {
                span[i] = 1;
            }

            // ちゃんと、2, 3, 4 番目だけが 1 になってる
            foreach (var x in array)
            {
                Console.WriteLine(x); // 0, 0, 1, 1, 1, 0, 0, 0
            }

            // 読み取り専用版
            ReadOnlySpan<int> r = span;
            var a = r[0]; // 読み取りは OK
#if InvalidCode
            r[0] = 1;     // 書き込みは NG
#endif
        }
    }
}
