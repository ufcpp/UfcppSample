namespace PatternBased.Deconstruct
{
    using BenchmarkDotNet.Attributes;
    using BenchmarkDotNet.Running;

    interface IDeconstructibleTo2Ints
    {
        void Deconstruct(out int x, out int y);
    }

    struct Point : IDeconstructibleTo2Ints
    {
        public int X { get; }
        public int Y { get; }
        public Point(int x, int y) => (X, Y) = (x, y);
        public void Deconstruct(out int x, out int y) => (x, y) = (X, Y);
    }

    [MemoryDiagnoser]
    public class DeconstructBenchmark
    {
        // Point を直接分解。
        // 最終的にインライン展開が働いて、単なる p.X + p.Y に展開される(ものすごく高速)。
        static int Sum1(Point p)
        {
            var (x, y) = p;
            return x + y;
        }

        // インターフェイスを介して分解。
        // インライン展開が効かず、ボックス化も起きてるので遅い。
        static int Sum2(IDeconstructibleTo2Ints p)
        {
            var (x, y) = p;
            return x + y;
        }

        [Benchmark]
        public int NoInterafce() => Sum1(new Point(1, 2));

        [Benchmark]
        public int WithInterafce() => Sum2(new Point(1, 2));
    }

    class Program
    {
        static void Main()
        {
            BenchmarkRunner.Run<DeconstructBenchmark>();
            //var b = new DeconstructBenchmark();
            //b.NoInterafce();
        }
    }
}
