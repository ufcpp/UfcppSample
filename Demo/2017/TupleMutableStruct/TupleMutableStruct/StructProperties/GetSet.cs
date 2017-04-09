namespace TupleMutableStruct.StructProperties.GetSet
{
    // mutable な構造体
    struct Point
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Point(int x, int y) => (X, Y) = (x, y);
    }

    struct Polygon
    {
        private Point[] _vertices;

        public Polygon(params Point[] vertices) => _vertices = vertices;

        // mutable な構造体をプロパティ/インデクサー越しに読み書き
        public Point this[int index]
        {
            get => _vertices[index];
            set => _vertices[index] = value;
        }
    }

    class Program
    {
        static void Main()
        {
            var p = new Polygon(
                new Point(0, 1),
                new Point(1, 1),
                new Point(1, 0)
                );

#if false
            // なぜか書き換えできない(実際にはそういう仕様)
            p[0].X = 2;
#endif

            // なぜか p[0].X が書き換わらない
            var v0 = p[0];
            v0.X = 2;

            // p[0] が返す値も、v0 で受け取った値もコピーなので、元の値が書き換わらなくて当然
        }
    }
}
