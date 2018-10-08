using System;

namespace IntTemplateParameter
{
    /// <summary>
    /// ガロアの有限体。
    /// </summary>
    /// <typeparam name="N">有限体のモジュロ。</typeparam>
    public struct GaloisField<N> : IEquatable<GaloisField<N>>
        where N : struct, IConstant<int>
    {
        /// <summary>
        /// 有限体のモジュロ。
        /// </summary>
        public static int Modulo => default(N).Value;

        private int _value;
        public GaloisField(int value) => _value = Mod(value);
        public override string ToString() => _value.ToString();

        public static explicit operator int(GaloisField<N> value) => value._value;
        public static implicit operator GaloisField<N>(int value) => new GaloisField<N>(value);

        public bool Equals(GaloisField<N> other) => _value == other._value;
        public override bool Equals(object obj) => obj is GaloisField<N> other && Equals(other);
        public override int GetHashCode() => _value.GetHashCode();
        public static bool operator == (GaloisField<N> x, GaloisField<N> y) => x.Equals(y);
        public static bool operator != (GaloisField<N> x, GaloisField<N> y) => !x.Equals(y);

        public GaloisField<N> Inverse() => new GaloisField<N>(Egcd(_value, default(N).Value).x);
        public static GaloisField<N> operator +(GaloisField<N> x) => x;
        public static GaloisField<N> operator -(GaloisField<N> x) => new GaloisField<N>(-x._value);

        public static GaloisField<N> operator +(GaloisField<N> x, GaloisField<N> y) => new GaloisField<N>(x._value + y._value);
        public static GaloisField<N> operator -(GaloisField<N> x, GaloisField<N> y) => new GaloisField<N>(x._value - y._value);
        public static GaloisField<N> operator *(GaloisField<N> x, GaloisField<N> y) => new GaloisField<N>(x._value * y._value);
        public static GaloisField<N> operator /(GaloisField<N> x, GaloisField<N> y) => x * y.Inverse();

        /// <summary>
        /// 拡張ユークリッド互除法で、a * x + b * y == 1 (mod gcd(a, b)) となるような x, y を計算する。
        /// b に N (素数前提)を与えることで、a * x == 1 (mode N) になるような x を計算で来て、a の逆元が求まる。
        /// </summary>
        static (int x, int y) Egcd(int a, int b)
        {
            if (b == 0) return (1, 0);
            else
            {
                var (y, x) = Egcd(b, a % b);
                y -= (a / b) * x;
                return (x, y);
            }
        }

        /// <summary>
        /// <paramref name="i"/>をNで割った余りを計算する。
        /// C# の % だと、i が負の時に結果が負になるので、その場合には N を足して正にする。
        /// </summary>
        static int Mod(int i)
        {
            i %= default(N).Value;
            if (i < 0) i += default(N).Value;
            return i;
        }
    }
}
