using System;
using Xunit;
using Table = TableMath.SinCosTableF;
using M = System.MathF;
using T = System.Single;

namespace TableMathTest
{
    public class SinCosTableTestF
    {
        private static readonly T[] _cornerCase = new[]
        {
            0,
            M.PI / 2,
            M.PI,
            3 * M.PI / 2,
            2 * M.PI,
            -M.PI / 2,
            -M.PI,
            -3 * M.PI / 2,
            -2 * M.PI,
        };

        [Fact]
        public void SinCosの戻り値はSinとCosと一致()
        {
            void assert(T theta)
            {
                var s1 = Table.Sin(theta);
                var c1 = Table.Cos(theta);
                var (s2, c2) = Table.SinCos(theta);

                Assert.Equal(s1, s2);
                Assert.Equal(c1, c2);
            }

            var r = new Random();
            for (int i = 0; i < 1000; i++)
            {
                var theta = (float)r.NextDouble() * 1000 - 500;
                assert(theta);
            }

            foreach (var theta in _cornerCase)
            {
                assert(theta);
            }
        }

        [Fact]
        public void テーブル上にある値は6桁精度で取れる()
        {
            void assert(T theta)
            {
                var s1 = Table.Sin(theta);
                var c1 = Table.Cos(theta);
                var s2 = M.Sin(theta);
                var c2 = M.Cos(theta);

                Assert.True(M.Abs(s1 - s2) < 1e-6f);
                Assert.True(M.Abs(c1 - c2) < 1e-6f);
            }

            for (int i = -256; i <= 256; i++)
            {
                var theta = 2 * M.PI / 256 * i;
                assert(theta);
            }

            foreach (var theta in _cornerCase)
            {
                assert(theta);
            }
        }

        [Fact]
        public void 精度が悪いところでも誤差2パーセント程度の精度で取れる()
        {
            void assert(T theta)
            {
                var s1 = Table.Sin(theta);
                var c1 = Table.Cos(theta);
                var s2 = M.Sin(theta);
                var c2 = M.Cos(theta);

                Assert.True(M.Abs(s1 - s2) < 1.3e-2f);
                Assert.True(M.Abs(c1 - c2) < 1.3e-2f);
            }

            for (int i = -256; i <= 256; i++)
            {
                var theta = 2 * M.PI / 256 * (i + 0.5f);
                assert(theta);
            }

            foreach (var theta in _cornerCase)
            {
                assert(theta);
            }
        }

        [Fact]
        public void Atan2誤差1パーセント以下の精度で計算できる()
        {
            void assertYX(T y, T x)
            {
                var t1 = Table.Atan2(y, x);
                var t2 = M.Atan2(y, x);

                Assert.True(M.Abs(t1 - t2) < 2e-3);
            }

            void assert(T theta)
            {
                var (y, x) = (M.Cos(theta), M.Sin(theta));
                assertYX(y, x);
            }

            var r = new Random();
            for (int i = 0; i < 1000; i++)
            {
                var theta = (float)r.NextDouble() * 1000 - 500;
                assert(theta);
            }
            for (int i = 0; i < 1000; i++)
            {
                var y = (float)r.NextDouble() * 1000 - 500;
                var x = (float)r.NextDouble() * 1000 - 500;
                assertYX(y, x);
            }

            foreach (var theta in _cornerCase)
            {
                assert(theta);
            }

            for (int x = -1; x <= 1; x++)
                for (int y = -1; y <= 1; y++)
                    assertYX(x, y);
        }
    }
}
