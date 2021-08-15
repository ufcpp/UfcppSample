namespace GenericMath.Algebra;

public readonly struct GaloisField<N> : IField<GaloisField<N>>
    where N : IConstant<int>
{
    private readonly int _value;

    public GaloisField(int value) => _value = Mod(value);
    public override string ToString() => _value.ToString();

    public static explicit operator int(GaloisField<N> value) => value._value;
    public static implicit operator GaloisField<N>(int value) => new(value);

    public bool Equals(GaloisField<N> other) => _value == other._value;
    public override bool Equals(object? obj) => obj is GaloisField<N> other && Equals(other);
    public override int GetHashCode() => _value.GetHashCode();
    public static bool operator ==(GaloisField<N> x, GaloisField<N> y) => x.Equals(y);
    public static bool operator !=(GaloisField<N> x, GaloisField<N> y) => !x.Equals(y);

    public static GaloisField<N> AdditiveIdentity => 0;
    public static GaloisField<N> MultiplicativeIdentity => 1;

    public GaloisField<N> Inverse() => new(Egcd(_value, N.Value).x);
    public static GaloisField<N> operator +(GaloisField<N> x) => x;
    public static GaloisField<N> operator -(GaloisField<N> x) => new(-x._value);

    public static GaloisField<N> operator +(GaloisField<N> x, GaloisField<N> y) => new(x._value + y._value);
    public static GaloisField<N> operator -(GaloisField<N> x, GaloisField<N> y) => new(x._value - y._value);
    public static GaloisField<N> operator *(GaloisField<N> x, GaloisField<N> y) => new(x._value * y._value);
    public static GaloisField<N> operator /(GaloisField<N> x, GaloisField<N> y) => x * y.Inverse();

    public static GaloisField<N> operator --(GaloisField<N> value) => new(value._value - 1);
    public static GaloisField<N> operator ++(GaloisField<N> value) => new(value._value + 1);

    /// <summary>
    /// Extended Euclidean Algorithm.
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
    /// <paramref name="i"/> mod N.
    /// </summary>
    private static int Mod(int i)
    {
        i %= N.Value;
        if (i < 0) i += N.Value;
        return i;
    }
}
