namespace ConsoleApplication1.RefReturns.RefParamsAndReturns
{
    class Program
    {
        static void Main()
        {
            var p = new Point { X = 2, Y = 3, Z = 5 };
            SomeAlgorithm(ref p);
            System.Console.WriteLine($"{p.X}, {p.Y}, {p.Z}"); // 4, 9, 25
        }

        /// <summary>
        /// X, Y, Z それぞれにまったく同じ処理を掛けたい場合がある。
        /// こういう、読み書きする場所だけ違って同じ計算をする場合、参照が使えると便利だったり。
        /// この例では X, Y, Z をそれぞれ二乗。
        /// </summary>
        /// <param name="p">計算対象。計算結果は自己書き換え。</param>
        static void SomeAlgorithm(ref Point p)
        {
            SomeAlgorithm(ref p, 0);
            SomeAlgorithm(ref p, 1);
            SomeAlgorithm(ref p, 2);
        }

        /// <summary>
        /// <see cref="SomeAlgorithm(ref Point)"/>の X, Y, Z を個別に計算する。
        /// </summary>
        /// <param name="p"></param>
        /// <param name="index">Xを計算したければ0、Yなら1、Zなら2</param>
        /// <returns></returns>
        static void SomeAlgorithm(ref Point p, int index)
        {
            ref var x = ref Get(ref p, index);
            x = x * x;
        }

        /// <summary>
        /// <see cref="Point"/>に対して、インデックス指定でX, Y, Zのいずれかへの参照を返す。
        /// </summary>
        /// <param name="p"></param>
        /// <param name="index">0ならX、1ならY、2ならZ</param>
        /// <returns></returns>
        static ref int Get(ref Point p, int index)
        {
            switch (index)
            {
                default:
                case 0: return ref p.X;
                case 1: return ref p.Y;
                case 2: return ref p.Z;
            }
        }
    }
}
