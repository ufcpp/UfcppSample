using System;
using System.Runtime.CompilerServices;

namespace TableMath
{
    /// <summary>
    /// unsafe で配列の range check さぼって速くできないかとやってみたやつ。
    /// 全然速くならなかった。
    /// t[i &amp; 255] で range check 消える最適化掛かるのかな、 .NET Core。
    /// </summary>
    public unsafe static class SinCosTableUnsafeF
    {
        private const float Factor = 128 / MathF.PI;
        private static readonly float* _cosTable;

        static SinCosTableUnsafeF()
        {
            var t = (float*)System.Runtime.InteropServices.Marshal.AllocHGlobal(256 * sizeof(float));
            for (int i = 0; i < 256; i++)
                t[i] = MathF.Cos(MathF.PI / 128 * i);
            _cosTable = t;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int Round(float x) => (int)MathF.Round(x);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Cos(float theta)
        {
            var i = Round(theta * Factor);
            return _cosTable[i & 0xff];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Sin(float theta)
        {
            var i = Round(theta * Factor);
            return _cosTable[(i - 64) & 0xff];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (float sin, float cos) SinCos(float theta)
        {
            var i = Round(theta * Factor);
            var t = _cosTable;
            return (t[(i - 64) & 0xff], t[i & 0xff]);
        }
    }
}
