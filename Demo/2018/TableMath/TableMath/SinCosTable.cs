using System;
using System.Runtime.CompilerServices;

namespace TableMath
{
    /// <summary>
    /// 極まってくると sin/cos はテーブルで持っておきたいというやつ。
    /// </summary>
    /// <remarks>
    /// sin/cos の角度の解像度は 256/1周。
    /// 剰余演算を &amp; 0xff で代用したいので、360度/1周 とかにはしない。
    /// sin/cos の値の精度的には、一番ずれてるところで2%以下くらいっぽい。
    /// </remarks>
    public static class SinCosTable
    {
        private const double Factor = 128 / Math.PI;
        private static readonly double[] _cosTable;

        static SinCosTable()
        {
            var t = new double[256];
            for (int i = 0; i < t.Length; i++)
                t[i] = Math.Cos(Math.PI / 128 * i);
            _cosTable = t;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int Round(double x) => (int)Math.Round(x);
        //=> (int)Math.Floor(x + 0.5); // Round と大して変わらなかった。Round (偶数丸め)十分速い

        /// <summary>
        /// cos。
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Cos(double theta)
        {
            var i = Round(theta * Factor);
            return _cosTable[i & 0xff];
        }

        /// <summary>
        /// sin。
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Sin(double theta)
        {
            var i = Round(theta * Factor);
            return _cosTable[(i - 64) & 0xff];
        }

        /// <summary>
        /// sin/cos を同時計算。
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (double sin, double cos) SinCos(double theta)
        {
            var i = Round(theta * Factor);
            var t = _cosTable;
            return (t[(i - 64) & 0xff], t[i & 0xff]);
        }

        /// <summary>
        /// sin/cos を同時計算。
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SinCos(double theta, out double sin, out double cos)
        {
            var i = Round(theta * Factor);
            var t = _cosTable;
            (sin, cos) = (t[(i - 64) & 0xff], t[i & 0xff]);
        }
    }
}
