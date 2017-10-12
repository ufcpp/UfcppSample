using System;

namespace SafeStackalloc
{
    class Program
    {
        static void Main()
        {
            // 適当に入力したデータ列だけど、意図的に 3 を一番多く含むようにしてある
            var data = new byte[] { 1, 2, 3, 4, 1, 34, 5, 7, 3, 5, 67, 2, 3, 45, 74, 3, 23, 63, 56, 8, 23, 12, 5, 23, 12, 64, 3, 12, 5, 23, 3, 76, 34, 75, 3, 86, 53, 2, 3, 124};

            // 以下の3つはいずれも頻度最大の値を返すメソッド
            // なので data を与えると 3 を返すはず
            // 違いは、出現数をカウントするための一時バッファーの取り方
            Console.WriteLine(SafeButSlow(data));
            Console.WriteLine(FastButUnsafe(data));
            Console.WriteLine(FastAndSafe(data));
        }

        static byte SafeButSlow(byte[] data)
        {
            // 安全なんだけど、この配列確保が結構負担
            var counts = new byte[256];

            foreach (var c in data) counts[c]++;

            return Max(counts);
        }

        static unsafe byte FastButUnsafe(byte[] data)
        {
            // 配列確保の負担はなくなるんだけど、ポインターが必須
            byte* counts = stackalloc byte[256];

            foreach (var c in data) counts[c]++;

            return Max(new Span<byte>(counts, 256));
        }

        static byte FastAndSafe(byte[] data)
        {
            // Span に対して代入するなら、unsafe を付けなくても stackalloc が使える
            // counts.Length はちゃんと 256 だし、インデクサーの範囲チェックもされる = buffer over run 脆弱性とかは避けれる = 安全
            Span<byte> counts = stackalloc byte[256];

            foreach (var c in data) counts[c]++;

            return Max(counts);
        }

        static byte Max(Span<byte> counts)
        {
            var max = 0;
            byte maxC = 0;
            for (int i = 0; i < 256; i++)
            {
                if (counts[i] > max)
                {
                    max = counts[i];
                    maxC = (byte)i;
                }
            }
            return maxC;
        }
    }
}
