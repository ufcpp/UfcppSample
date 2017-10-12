namespace ReadOnlyStruct
{
    struct Point
    {
        // フィールドに readonly を付けているものの…
        public readonly int X;
        public readonly int Y;

        public Point(int x, int y) => (X, Y) = (x, y);

        // this の書き換えができてしまうので、実は X, Y の書き換えが可能
        public void Set(int x, int y) => this = new Point(x, y);
    }

    readonly struct ReadOnlyPoint
    {
        public readonly int X;
        public readonly int Y;

        public ReadOnlyPoint(int x, int y) => (X, Y) = (x, y);

#if InvalidCode
    // readonly struct なら、これがコンパイル エラーになる
    public void Set(int x, int y) => this = new Point(x, y);
#endif
    }

    class Program
    {
        static void Main()
        {
            var p = new Point(1, 2);
            p.Set(3, 4); // readonly フィールドの書き換えが出来ちゃう

            System.Console.WriteLine(p.X); // 3
            System.Console.WriteLine(p.Y); // 4

            var r = new ReadOnlyPoint(1, 2);
            System.Console.WriteLine(r.X); // こっちなら、絶対に 1 な保証あり
            System.Console.WriteLine(r.Y); // 同、2
        }
    }
}
