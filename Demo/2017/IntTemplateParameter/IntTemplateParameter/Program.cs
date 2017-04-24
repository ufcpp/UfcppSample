using System;
using static IntTemplateParameter.ConstantInt;

namespace IntTemplateParameter
{
    using Gf2 = GaloisField<_2>;
    using Gf3 = GaloisField<_3>;
    using Gf5 = GaloisField<_5>;
    using Gf7 = GaloisField<_7>;

    class Program
    {
        static void Main(string[] args)
        {
            TestGf2();
            TestGf3();
            TestGf5();
            TestGf7();
        }

        static void TestGf2()
        {
            Gf2 x = 0;
            AssertEqual(x + 1, 1);
            AssertEqual(x * 1, 0);

            x = 1;
            AssertEqual(x.Inverse(), 1);
            AssertEqual(x + 1, 0);
            AssertEqual(x * 1, 1);

            for (int i = 1; i < Gf2.Modulo; i++)
            {
                // N 回足したら必ず 0
                Gf2 s = 0;
                for (int n = 0; n < Gf2.Modulo; n++) s += i;
                AssertEqual(s, 0);

                // N - 1 回掛けたら必ず 1
                Gf2 p = 1;
                for (int n = 0; n < Gf2.Modulo - 1; n++) p *= i;
                AssertEqual(p, 1);
            }
        }

        static void TestGf3()
        {
            Gf3 x = 0;
            AssertEqual(x + 1, 1);
            AssertEqual(x * 1, 0);

            x = 1;
            AssertEqual(x.Inverse(), 1);
            AssertEqual(x + 1, 2);
            AssertEqual(x + 2, 0);
            AssertEqual(x * 1, 1);
            AssertEqual(x * 2, 2);

            x = 2;
            AssertEqual(x.Inverse(), 2);
            AssertEqual(x + 1, 0);
            AssertEqual(x + 2, 1);
            AssertEqual(x * 1, 2);
            AssertEqual(x * 2, 1);

            for (int i = 1; i < Gf3.Modulo; i++)
            {
                // N 回足したら必ず 0
                Gf3 s = 0;
                for (int n = 0; n < Gf3.Modulo; n++) s += i;
                AssertEqual(s, 0);

                // N - 1 回掛けたら必ず 1
                Gf3 p = 1;
                for (int n = 0; n < Gf3.Modulo - 1; n++) p *= i;
                AssertEqual(p, 1);
            }
        }

        static void TestGf5()
        {
            Gf5 x = 0;
            AssertEqual(x + 1, 1);
            AssertEqual(x * 1, 0);

            x = 2;
            AssertEqual(x.Inverse(), 3);
            AssertEqual(x + 1, 3);
            AssertEqual(x + 4, 1);
            AssertEqual(x * 2, 4);
            AssertEqual(x * 4, 3);

            x = 3;
            AssertEqual(x.Inverse(), 2);
            AssertEqual(x + 4, 2);
            AssertEqual(x + 2, 0);
            AssertEqual(x * 3, 4);
            AssertEqual(x * 2, 1);

            for (int i = 1; i < Gf5.Modulo; i++)
            {
                // N 回足したら必ず 0
                Gf5 s = 0;
                for (int n = 0; n < Gf5.Modulo; n++) s += i;
                AssertEqual(s, 0);

                // N - 1 回掛けたら必ず 1
                Gf5 p = 1;
                for (int n = 0; n < Gf5.Modulo - 1; n++) p *= i;
                AssertEqual(p, 1);
            }
        }

        static void TestGf7()
        {
            Gf7 x = 0;
            AssertEqual(x + 1, 1);
            AssertEqual(x * 1, 0);

            x = 3;
            AssertEqual(x.Inverse(), 5);
            AssertEqual(x + 6, 2);
            AssertEqual(x + 3, 6);
            AssertEqual(x * 2, 6);
            AssertEqual(x * 4, 5);

            x = 6;
            AssertEqual(x.Inverse(), 6);
            AssertEqual(x + 4, 3);
            AssertEqual(x + 2, 1);
            AssertEqual(x * 3, 4);
            AssertEqual(x * 2, 5);

            for (int i = 1; i < Gf7.Modulo; i++)
            {
                // N 回足したら必ず 0
                Gf7 s = 0;
                for (int n = 0; n < Gf7.Modulo; n++) s += i;
                AssertEqual(s, 0);

                // N - 1 回掛けたら必ず 1
                Gf7 p = 1;
                for (int n = 0; n < Gf7.Modulo - 1; n++) p *= i;
                AssertEqual(p, 1);
            }
        }

        static void AssertEqual<T>(T x, T y)
            where T : IEquatable<T>
        {
            if (!x.Equals(y)) throw new InvalidOperationException();
        }
    }
}
