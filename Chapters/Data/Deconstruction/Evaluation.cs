using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deconstruction.Evaluation
{
    class Program
    {
        static void Main()
        {
            var t = ("abc", 100);

            FailureSwap();
            TupleSwap();
            TupleSwapEval();
            SideEffect();
            SideEffectEval();
        }

        private static void FailureSwap()
        {
            var x = 1;
            var y = 2;

            y = x;
            x = y; // 上の行で y が書き換わっているので、値の入れ替えにはならない

            Console.WriteLine(x); // 1
            Console.WriteLine(y); // 1

            // 正しくは以下のように書く
            // var temp = y;
            // y = x;
            // x = temp;
        }

        private static void TupleSwap()
        {
            var x = 1;
            var y = 2;

            // 分解代入であれば、値の書き換えは同時に起こる
            (y, x) = (x, y); // 一時的にタプルが作られる

            Console.WriteLine(x); // 2
            Console.WriteLine(y); // 1
        }

        private static void TupleSwapEval()
        {
            var x = 1;
            var y = 2;

            // 実際には、同時に書き換わったように見えるように、一時変数が挟まる
            // (y, x) = (x, y) であれば、以下のように評価されてる
            var t = (x, y);
            y = t.Item1;
            x = t.Item2;

            Console.WriteLine(x); // 2
            Console.WriteLine(y); // 1
        }

        private static void SideEffect()
        {
            var a = new[] { 0, 1, 2, 3 };
            var i = 0;

            (a[i++], a[i++]) = (a[i++], a[i++]);

            Console.WriteLine(string.Join(", ", a)); // 2, 3, 2, 3
            // つまり、以下の評価を受けてる
            // (a[0], a[1]) = (a[2], a[3]);
        }

        private static void SideEffectEval()
        {
            var a = new[] { 0, 1, 2, 3 };
            var i = 0;

            ref var l1 = ref a[i++];
            ref var l2 = ref a[i++];
            var r1 = a[i++];
            var r2 = a[i++];

            l1 = r1;
            l2 = r2;

            Console.WriteLine(string.Join(", ", a)); // 2, 3, 2, 3
        }
    }
}
