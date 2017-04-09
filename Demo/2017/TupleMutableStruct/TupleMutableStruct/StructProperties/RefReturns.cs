namespace TupleMutableStruct.StructProperties.RefReturns
{
    using Point = GetSet.Point;

    // Point は元と一緒

    struct Polygon
    {
        private Point[] _vertices;

        public Polygon(params Point[] vertices) => _vertices = vertices;

        // 構造体を mutable にしたいときってのは、大体パフォーマンス優先の時
        // そういう時には ref returns が使える
        public ref Point this[int index] => ref _vertices[index];
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

            // ちゃんと書き換わる
            p[0].X = 2;

            // ちゃんと p[0].X が書き換わる
            ref var v0 = ref p[0];
            v0.X = 2;
        }
    }
}
