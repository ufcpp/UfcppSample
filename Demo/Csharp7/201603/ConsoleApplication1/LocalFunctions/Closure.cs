using System;

namespace ConsoleApplication1.LocalFunctions
{
    class Closure
    {
        static void Main()
        {
            LocalFunction1.F();
            LocalFunction2.F();
            LambdaExression1.F();
            LambdaExression2.F();
        }

        class LocalFunction1
        {
            public static void F()
            {
                int x = 0;
                void f(int n) => x = n;
                f(10);
                Console.WriteLine(x);
            }
        }

        class LocalFunction2
        {
            public static void F()
            {
                var s = new FState { x = 0 };
                F_f(10, ref s);
                Console.WriteLine(s.x);
            }

            struct FState
            {
                public int x;
            }

            static void F_f(int n, ref FState s) => s.x = n;
        }

        class LambdaExression1
        {
            public static void F()
            {
                int x = 0;
                Action<int> f = n => x = n;
                f(10);
                Console.WriteLine(x);
            }
        }

        class LambdaExression2
        {
            public static void F()
            {
                var s = new FState { x = 0 };
                Action<int> f = s.f;
                f(10);
                Console.WriteLine(s.x);
            }

            class FState
            {
                public int x;

                public void f(int n) => x = n;
            }
        }
    }
}
