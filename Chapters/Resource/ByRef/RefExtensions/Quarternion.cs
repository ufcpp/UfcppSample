namespace ByRef.RefExtensions
{
    public static class QuarternionExtensions
    {
        // 構造体の書き換えを拡張メソッドでやりたい場合に ref 引数が使える
        public static void Conjugate(ref this Quarternion q)
        {
            var norm = q.W * q.W + q.X * q.X + q.Y * q.Y + q.Z * q.Z;
            q.W = q.W / norm;
            q.X = -q.X / norm;
            q.Y = -q.Y / norm;
            q.Z = -q.Z / norm;
        }

        // コピーを避けたい場合に in 引数が使える
        public static Quarternion Rotate(in this Quarternion p, in Quarternion q)
        {
            var qc = q;
            qc.Conjugate();
            return q * p * qc;
        }
    }

    public struct Quarternion
    {
        public double W;
        public double X;
        public double Y;
        public double Z;
        public Quarternion(double w, double x, double y, double z) => (W, X, Y, Z) = (w, x, y, z);

        public static Quarternion operator *(in Quarternion a, in Quarternion b)
            => new Quarternion(
                a.W * b.W - a.X * b.X - a.Y * b.Y - a.Z * b.Z,
                a.W * b.X + a.X * b.W + a.Y * b.Z - a.Z * b.Y,
                a.W * b.Y + a.Y * b.W + a.Z * b.X - a.X * b.Z,
                a.W * b.Z + a.Z * b.W + a.X * b.Y - a.Y * b.X);
    }
}
