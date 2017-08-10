namespace PropertyAccessor.SampleData
{
    public struct Point
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Point(int x, int y) : this()
        {
            X = x;
            Y = y;
        }

        public static bool Equals(Point x, Point y) => x.X == y.X && x.Y == y.Y;
    }
}
