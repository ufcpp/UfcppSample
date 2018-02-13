using BenchmarkDotNet.Running;

class Program
{
    static void Main()
    {
        var b = new MatrixBenchmark();
        var x = b.ByValue();
        var y = b.ByRef();
        var z = b.Simd();

        unsafe
        {
            var px = (float*)&x;
            var py = (float*)&y;
            var pz = (float*)&z;

            for (int i = 0; i < 16; i++)
            {
                System.Console.WriteLine((px[i] - py[i], px[i] - pz[i], px[i], py[i], pz[i]));
            }
        }

        BenchmarkRunner.Run<MatrixBenchmark>();
    }
}
