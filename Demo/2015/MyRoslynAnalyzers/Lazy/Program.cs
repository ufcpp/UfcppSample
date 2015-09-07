using System;

namespace Lazy
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("1");

            var s = new Sample();

            Console.WriteLine("2");

            var x = s.X; // ここで初めて X のコンストラクターが呼ばれる

            Console.WriteLine("3");

/*
結果:

1
2
X のコンストラクター
3
*/
        }
    }
}
