namespace Class.Immutable
{
    /// <summary>
    /// クラス(参照型) かつ 書き換え不能
    /// </summary>
    public class Vector
    {
        public readonly double A, B, C, D, E, F, G, H;

        public Vector() { }

        public Vector(double a, double b, double c, double d, double e, double f, double g, double h)
        {
            A = a;
            B = b;
            C = c;
            D = d;
            E = e;
            F = f;
            G = g;
            H = h;
        }

        public Vector Add(Vector v) => new Vector(A + v.A, B + v.B, C + v.C, D + v.D, E + v.E, F + v.F, G + v.G, H + v.H);
    }
}
