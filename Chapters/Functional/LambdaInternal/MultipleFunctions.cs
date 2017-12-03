namespace LambdaInternal.MultipleFunctions
{
    using System;

    class Program
    {
        static void F(int m)
        {
            // ローカル関数かラムダ式か匿名デリゲート式かは無関係
            void a(int x) => Console.WriteLine("A " + m * x);
            Action<int> b = x => Console.WriteLine("B " + m * x);
            Action<int> c = delegate (int x) { Console.WriteLine("C " + m * x); };

            Invoke(a, b, c);
        }

        static void Invoke(params Action<int>[] list)
        {
            foreach (var item in list) item(1);
        }
    }

    namespace Compiled
    {
        using System;

        // a, b, c いずれも1つの型にまとまる
        class AnonymousClass
        {
            public int m;
            internal void A(int x) => Console.WriteLine("A " + x);
            internal void B(int x) => Console.WriteLine("B " + x);
            internal void C(int x) => Console.WriteLine("C " + x);
        }

        class Program
        {
            static void F(int m)
            {
                // 作られるインスタンスは1つだけ
                var anonymous = new AnonymousClass();
                anonymous.m = m;

                Action<int> a = anonymous.A;
                Action<int> b = anonymous.B;
                Action<int> c = anonymous.C;

                Invoke(a, b, c);
            }

            static void Invoke(params Action<int>[] list)
            {
                foreach (var item in list) item(1);
            }
        }
    }
}
