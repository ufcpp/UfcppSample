namespace Build2014.Csharp5
{
    /// <summary>
    /// (X, Y) なクラス。X も Y も immutable に作りたい。
    /// その場合、結構煩雑な定型文になってしまう…
    /// </summary>
    class Point
    {
        private int _x; // ←ここと
        private int _y;

        public Point(int x, int y) // ←ここと
        {
            _x = x; // ←ここと
            _y = y;
        }

        public int X { get { return _x; } } // ←ここ、何回 x 書けばいいんだよ！
        public int Y { get { return _y; } }

        public override string ToString()
        {
            return string.Format("({0}, {1})", X, Y);
        }
    }
}
