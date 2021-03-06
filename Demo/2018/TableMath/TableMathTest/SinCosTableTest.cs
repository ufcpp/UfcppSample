﻿using System;
using Xunit;
using Table = TableMath.SinCosTable;
using M = System.Math;
using T = System.Double;

namespace TableMathTest
{
    public class SinCosTableTest
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
                var theta = r.NextDouble() * 1000 - 500;
                assert(theta);
            }

            foreach (var theta in _cornerCase)
            {
                assert(theta);
            }
        }

        [Fact]
        public void テーブル上にある値は15桁精度で取れる()
        {
            void assert(T theta)
            {
                var s1 = Table.Sin(theta);
                var c1 = Table.Cos(theta);
                var s2 = M.Sin(theta);
                var c2 = M.Cos(theta);

                Assert.True(M.Abs(s1 - s2) < 1e-15);
                Assert.True(M.Abs(c1 - c2) < 1e-15);
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

                Assert.True(M.Abs(s1 - s2) < 1.3e-2);
                Assert.True(M.Abs(c1 - c2) < 1.3e-2);
            }

            for (int i = -256; i <= 256; i++)
            {
                var theta = 2 * M.PI / 256 * (i + 0.5);
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
                var theta = r.NextDouble() * 1000 - 500;
                assert(theta);
            }
            for (int i = 0; i < 1000; i++)
            {
                var y = r.NextDouble() * 1000 - 500;
                var x = r.NextDouble() * 1000 - 500;
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

        [Fact]
        public void Angleでの値は15桁精度で取れる()
        {
            void assert(Table.Angle angle)
            {
                var s1 = Table.Sin(angle);
                var c1 = Table.Cos(angle);
                var theta = angle.ToRadian();
                var s2 = M.Sin(theta);
                var c2 = M.Cos(theta);

                Assert.True(M.Abs(s1 - s2) < 1e-15);
                Assert.True(M.Abs(c1 - c2) < 1e-15);
            }

            for (int i = 0; i < 256; i++)
            {
                assert(new Table.Angle((byte)i));
            }
        }

        [Fact]
        public void SinCosからAtan2で元のAngleを復元()
        {
            void assert(Table.Angle angle)
            {
                var s1 = Table.Sin(angle);
                var c1 = Table.Cos(angle);
                var atan = Table.Atan2(s1, c1);
                var a1 = Table.Angle.FromRadian(atan);

                Assert.Equal(angle, a1);
            }

            for (int i = 0; i < 256; i++)
            {
                assert(new Table.Angle((byte)i));
            }
        }
    }
}
