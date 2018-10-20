using System;
using TableMath;
using Xunit;

namespace TableMathTest
{
    public class SinCosTableTest
    {
        [Fact]
        public void SinCosの戻り値はSinとCosと一致()
        {
            var r = new Random();
            for (int i = 0; i < 1000; i++)
            {
                var theta = r.NextDouble() * 1000 - 500;
                var s1 = SinCosTable.Sin(theta);
                var c1 = SinCosTable.Cos(theta);
                var (s2, c2) = SinCosTable.SinCos(theta);

                Assert.Equal(s1, s2);
                Assert.Equal(c1, c2);
            }
        }

        [Fact]
        public void テーブル上にある値は15桁精度で取れる()
        {
            for (int i = -256; i <= 256; i++)
            {
                var theta = 2 * Math.PI / 256 * i;
                var s1 = SinCosTable.Sin(theta);
                var c1 = SinCosTable.Cos(theta);
                var s2 = Math.Sin(theta);
                var c2 = Math.Cos(theta);

                Assert.True(Math.Abs(s1 - s2) < 1e-15);
                Assert.True(Math.Abs(c1 - c2) < 1e-15);
            }
        }

        [Fact]
        public void 精度が悪いところでも誤差2パーセント程度の精度で取れる()
        {
            for (int i = -256; i <= 256; i++)
            {
                var theta = 2 * Math.PI / 256 * (i + 0.5);
                var s1 = SinCosTable.Sin(theta);
                var c1 = SinCosTable.Cos(theta);
                var s2 = Math.Sin(theta);
                var c2 = Math.Cos(theta);

                Assert.True(Math.Abs(s1 - s2) < 2e-2);
                Assert.True(Math.Abs(c1 - c2) < 2e-2);
            }
        }
    }
}
