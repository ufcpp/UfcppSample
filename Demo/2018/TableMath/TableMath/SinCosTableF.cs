using System;
using System.Runtime.CompilerServices;

namespace TableMath
{
    /// <summary>
    /// float 版。
    /// <see cref="SinCosTable"/>
    /// </summary>
    public static class SinCosTableF
    {
        private const float Factor = 128 / MathF.PI;
        private static readonly float[] _cosTable;

        static SinCosTableF()
        {
            var t = new float[256];
            for (int i = 0; i < 256; i++)
                t[i] = MathF.Cos(MathF.PI / 128 * i);
            _cosTable = t;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int Round(float x) => (int)MathF.Round(x);

        /// <summary>
        /// cos。
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Cos(float theta)
        {
            var i = Round(theta * Factor);
            return _cosTable[i & 0xff];
        }

        /// <summary>
        /// sin。
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Sin(float theta)
        {
            var i = Round(theta * Factor);
            return _cosTable[(i - 64) & 0xff];
        }

        /// <summary>
        /// sin/cos を同時計算。
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (float sin, float cos) SinCos(float theta)
        {
            var i = Round(theta * Factor);
            var t = _cosTable;
            return (t[(i - 64) & 0xff], t[i & 0xff]);
        }

        // double 版だとタプル (double sin, double cos) で返しても十分速いんだけど、
        // なぜか float 版タプル戻り値が遅い。
        // タプル構築の最適化が double の方が掛かりやすいっぽい？なので、out 引数で返してみる。
        // out 引数の方は想定通りの速度(Sin, Cos 単体と遜色ない速度)出てた。

        /// <summary>
        /// sin/cos を同時計算。
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SinCos(float theta, out float sin, out float cos)
        {
            var i = Round(theta * Factor);
            var t = _cosTable;
            (sin, cos) = (t[(i - 64) & 0xff], t[i & 0xff]);
        }
    }
}
