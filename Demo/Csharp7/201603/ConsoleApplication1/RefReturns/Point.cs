namespace ConsoleApplication1.RefReturns
{
    public struct Point
    {
        public int X;
        public int Y;
        public int Z;

        public Point(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public override string ToString() => $"({X}, {Y}, {Z})";
    }
}