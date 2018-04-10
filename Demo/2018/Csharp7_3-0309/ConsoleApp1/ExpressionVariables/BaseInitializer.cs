using System;

namespace ConsoleApp1.ExpressionVariables
{
    class Base
    {
        protected Base(out int x)
        {
            x = 999;
        }
    }

    class Derived : Base
    {
        // ThisInitializer でも触れたとおり、コンストラクター初期化子で out var できるように。
        // このとき、コンストラクター自体が out で値を返しているのを out var で受け取ることもできるし、
        // 基底クラスの base 呼び出しでも out var 可能。
        public Derived()
            : base(out var x)
        {
            // 初期化子中で宣言した変数 x はコンストラクター内でも使える。
            Console.WriteLine(x);
        }
    }

    class BaseInitializer
    {
        static void Main()
        {
            new Derived(); // Derived のコンストラクター内の WriteLine が呼ばれて、999 と表示される
        }
    }
}
