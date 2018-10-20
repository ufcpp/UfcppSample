using System;
using TableMath;
using Xunit;

namespace TableMathTest
{
    public class SinCosTableFTest
    {
        [Fact]
        public void SinCosの戻り値はSinとCosと一致()
        {
            var r = new Random();
            for (int i = 0; i < 1000; i++)
            {
                var theta = (float)r.NextDouble() * 1000 - 500;
                var s1 = SinCosTableF.Sin(theta);
                var c1 = SinCosTableF.Cos(theta);
                var (s2, c2) = SinCosTableF.SinCos(theta);
                SinCosTableF.SinCos(theta, out var s3, out var c3);

                Assert.Equal(s1, s2);
                Assert.Equal(c1, c2);

                Assert.Equal(s1, s3);
                Assert.Equal(c1, c3);
            }
        }

        [Fact]
        public void テーブル上にある値は6ケタ程度の精度で取れる()
        {
            for (int i = -256; i <= 256; i++)
            {
                var theta = 2 * MathF.PI / 256 * i;
                var s1 = SinCosTableF.Sin(theta);
                var c1 = SinCosTableF.Cos(theta);
                var s2 = MathF.Sin(theta);
                var c2 = MathF.Cos(theta);

                Assert.True(MathF.Abs(s1 - s2) < 1e-6);
                Assert.True(MathF.Abs(c1 - c2) < 1e-6);
            }
        }

        [Fact]
        public void 精度が悪いところでも誤差2パーセント程度の精度で取れる()
        {
            for (int i = -256; i <= 256; i++)
            {
                var theta = 2 * MathF.PI / 256 * (i + 0.5f);
                var s1 = SinCosTableF.Sin(theta);
                var c1 = SinCosTableF.Cos(theta);
                var s2 = MathF.Sin(theta);
                var c2 = MathF.Cos(theta);

                Assert.True(MathF.Abs(s1 - s2) < 2e-2);
                Assert.True(MathF.Abs(c1 - c2) < 2e-2);
            }
        }
    }
}
