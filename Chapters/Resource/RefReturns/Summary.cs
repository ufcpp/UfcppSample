namespace RefReturns.Summary
{
    using System;

    class Program
    {
        static void Main()
        {
            var x = 10;
            var y = 20;

            // x, y のうち、大きい方の参照を返す。この例の場合 y を参照。
            ref var m = ref Max(ref x, ref y);

            // 参照の書き換えなので、その先の y が書き換わる。
            m = 0;

            Console.WriteLine($"{x}, {y}"); // 10, 0
        }

        static ref int Max(ref int x, ref int y)
        {
            if (x < y) return ref y;
            else return ref x;
        }
    }
}
