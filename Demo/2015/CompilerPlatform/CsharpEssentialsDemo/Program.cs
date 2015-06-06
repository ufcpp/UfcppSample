namespace CsharpEssentialsDemo
{
    class Program
    {
        static void Main(string[] args)
        {
        }
    }

    public class Point
    {
        public int X { get; private set; }

        public int Y { get { return _y; } }
        private int _y;

        public Point(int x, int y)
        {
            X = x;
            _y = y;
        }

        public override string ToString()
        {
            return $"({X}, {Y})";
        }
    }
}
