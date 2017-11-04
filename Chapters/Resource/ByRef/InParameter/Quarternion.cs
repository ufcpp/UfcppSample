namespace ByRef.InParameter
{
    public struct Quarternion
    {
        public double W;
        public double X;
        public double Y;
        public double Z;
        public Quarternion(double w, double x, double y, double z) => (W, X, Y, Z) = (w, x, y, z);

        // 足し算4つくらいならインライン展開されて、値渡しでもコピーのコストが掛からない
        public static Quarternion operator +(Quarternion a, Quarternion b)
            => new Quarternion(
                a.W + b.W,
                a.X + b.X,
                a.Y + b.Y,
                a.Z + b.Z);

        // このくらい中身が大きい(掛け算16個、足し算9個)と、インライン展開されないので in 引数にする効果が結構出る
        public static Quarternion operator *(in Quarternion a, in Quarternion b)
            => new Quarternion(
                a.W * b.W - a.X * b.X - a.Y * b.Y - a.Z * b.Z,
                a.W * b.X + a.X * b.W + a.Y * b.Z - a.Z * b.Y,
                a.W * b.Y + a.Y * b.W + a.Z * b.X - a.X * b.Z,
                a.W * b.Z + a.Z * b.W + a.X * b.Y - a.Y * b.X);
    }
}
