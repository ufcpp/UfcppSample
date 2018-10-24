using System;
using System.Runtime.CompilerServices;
using M = System.Math;
using T = System.Double;

namespace TableMath
{
    /// <summary>
    /// 極まってくると sin/cos はテーブルで持っておきたいというやつ。
    /// </summary>
    /// <remarks>
    /// sin/cos の角度の解像度は 256/1周。
    /// 剰余演算を (x &amp; 0xff) で代用したいので、360度/1周 とかにはしない。
    /// sin/cos の値の精度的には、一番ずれてるところで1.3%以下くらいっぽい。
    /// </remarks>
    public static class SinCosTable
    {
        private const T PI = M.PI;
        private const T CosTableFactor = 128 / PI; // 2π で 256
        private const T AtanTableFactor = 255; // y/x == 1 で 255
        private const T invCosTableFactor = 1 / CosTableFactor;
        private const T invAtanTableFactor = 1 / AtanTableFactor;
        private static readonly T[] _cosTable;
        private static readonly T[] _atanTable;

        static SinCosTable()
        {
            var ct = new T[256];
            var att = new T[256];
            for (int i = 0; i < ct.Length; i++)
            {
                ct[i] = M.Cos(invCosTableFactor * i);
                att[i] = M.Atan(invAtanTableFactor * i);
            }
            _cosTable = ct;
            _atanTable = att;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int Round(T x) => (int)M.Round(x);
        //=> (int)Floor(x + 0.5); // Round と大して変わらなかった。Round (偶数丸め)十分速い

        /// <summary>
        /// cos。
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Cos(T theta)
        {
            var i = Round(theta * CosTableFactor);
            return _cosTable[i & 0xff];
        }

        /// <summary>
        /// sin。
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Sin(T theta)
        {
            var i = Round(theta * CosTableFactor);
            return _cosTable[(i - 64) & 0xff];
        }

        /// <summary>
        /// sin/cos を同時計算。
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (T sin, T cos) SinCos(T theta)
        {
            var i = Round(theta * CosTableFactor);
            var t = _cosTable;
            return (t[(i - 64) & 0xff], t[i & 0xff]);
        }

        /// <summary>
        /// sin/cos を同時計算。
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SinCos(T theta, out T sin, out T cos)
        {
            var i = Round(theta * CosTableFactor);
            var t = _cosTable;
            (sin, cos) = (t[(i - 64) & 0xff], t[i & 0xff]);
        }

        /// <summary>
        /// atan(y / x)。
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Atan2(T y, T x)
        {
            var negX = x < 0;
            if (negX) x = -x;

            var negY = y < 0;
            if (negY) y = -y;

            if (x == 0)
            {
                if (y == 0) return 0;
                else return negY ? -PI / 2 : PI / 2;
            }
            else if (y == 0) return negX ? PI : 0;

            var t = _atanTable;
            var atan = x > y ?
                t[Round(y / x * AtanTableFactor)] :
                PI / 2 - t[Round(x / y * AtanTableFactor)];

            if (negX) atan = PI - atan;
            if (negY) atan = -atan;

            return atan;
        }

        public readonly struct Angle : IEquatable<Angle>
        {
            internal readonly byte _index;
            public Angle(byte index) => _index = index;

            public T ToRadian() => _index * invCosTableFactor;
            public static Angle FromRadian(T angle) => new Angle(unchecked((byte)Round(angle * CosTableFactor)));
            public static Angle FromDegree(T angle) => new Angle(unchecked((byte)Round(angle * 256 / 360)));
            public static explicit operator Angle(T angle) => FromRadian(angle);

            public bool Equals(Angle other) => _index == other._index;
            public override bool Equals(object obj) => obj is Angle other && Equals(other);
            public override int GetHashCode() => _index;
            public static bool operator ==(Angle x, Angle y) => x._index == y._index;
            public static bool operator !=(Angle x, Angle y) => x._index != y._index;
        }

        /// <summary>
        /// cos。
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Cos(Angle angle)
        {
            var i = angle._index;
            return _cosTable[i];
        }

        /// <summary>
        /// sin。
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Sin(Angle angle)
        {
            var i = angle._index;
            return _cosTable[(i - 64) & 0xff];
        }

        /// <summary>
        /// sin/cos を同時計算。
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SinCos(Angle angle, out T sin, out T cos)
        {
            var i = angle._index;
            var t = _cosTable;
            (sin, cos) = (t[(i - 64) & 0xff], t[i]);
        }
    }
}
