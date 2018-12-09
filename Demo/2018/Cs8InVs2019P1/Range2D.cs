namespace Cs8InVs2019P1.Range2D
{
    using System;

    static class Matrix
    {
        public static int[,] Slice(this int[,] m,
            int xMinInclusive, int xMaxExclusive,
            int yMinInclusive, int yMaxExclusive)
        {
            var n = new int[yMaxExclusive - yMinInclusive, xMaxExclusive - xMinInclusive];

            for (int y = yMinInclusive, j = 0; y < yMaxExclusive; y++, j++)
                for (int x = xMinInclusive, i = 0; x < xMaxExclusive; x++, i++)
                    n[j, i] = m[y, x];

            return n;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var m = new[,]
            {
                { 1, 2, 3, 4 },
                { 5, 6, 7, 8 },
                { 9, 10, 11, 12 },
            };

            // (x, y) が (1, 2) ～ (3, 4) の範囲？
            // x が 1～2、y が 3～4 の範囲？
            // 2, 4 は含む？含まない？
            var n = m.Slice(1, 2, 3, 4);

            foreach (int x in n)
            {
                Console.WriteLine(x);
            }

            var i = 2;
            var r = i..i - 1;
        }
    }
}
